using StackExchange.Redis;
using System.Text.Json;

namespace listings_service.Infrastructure.Services
{
    public class RedisListingsRepository
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _cache;
        private readonly ILogger<RedisListingsRepository> _logger;
        private readonly TimeSpan _defaultExpiryTime;

        public RedisListingsRepository(IConnectionMultiplexer redis, IConfiguration configuration, ILogger<RedisListingsRepository> logger)
        {
            _redis = redis;
            _cache = redis.GetDatabase();
            _logger = logger;
            
            // Get default expiry time from configuration, or use 1 hour as default
            var expiryMinutes = configuration.GetValue<int>("RedisSettings:DefaultExpiryTimeInMinutes", 60);
            _defaultExpiryTime = TimeSpan.FromMinutes(expiryMinutes);
        }

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            try
            {
                var value = await _cache.StringGetAsync(key);
                if (value.IsNullOrEmpty)
                {
                    return null;
                }

                return JsonSerializer.Deserialize<T>(value!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving object from Redis cache for key: {Key}", key);
                return null;
            }
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null) where T : class
        {
            try
            {
                var serializedValue = JsonSerializer.Serialize(value);
                return await _cache.StringSetAsync(key, serializedValue, expiry ?? _defaultExpiryTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving object to Redis cache for key: {Key}", key);
                return false;
            }
        }

        public async Task<bool> RemoveAsync(string key)
        {
            try
            {
                return await _cache.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing key from Redis cache: {Key}", key);
                return false;
            }
        }

        public async Task<bool> RemoveByPrefixAsync(string prefix)
        {
            try
            {
                var endpoints = _redis.GetEndPoints();
                var server = _redis.GetServer(endpoints.First());
                var keys = server.Keys(pattern: $"{prefix}*").ToArray();
                
                if (keys.Length == 0)
                    return true;
                    
                var deletedCount = await _cache.KeyDeleteAsync(keys);
                return deletedCount == keys.Length;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing keys by prefix from Redis cache: {Prefix}", prefix);
                return false;
            }
        }
    }
}
