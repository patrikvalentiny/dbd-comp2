using MongoDB.Driver;
using listings_service.Infrastructure.Contexts;

namespace listings_service.Infrastructure.Services
{
    public class MongoTransactionManager : IMongoTransactionManager
    {
        private readonly MongoContext _context;
        private readonly ILogger<MongoTransactionManager> _logger;

        public MongoTransactionManager(MongoContext context, ILogger<MongoTransactionManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IClientSessionHandle> BeginTransactionAsync()
        {
            try
            {
                var session = await _context.Client.StartSessionAsync();
                session.StartTransaction();
                return session;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting MongoDB transaction");
                throw;
            }
        }

        public async Task CommitTransactionAsync(IClientSessionHandle session)
        {
            try
            {
                await session.CommitTransactionAsync();
            }
            finally
            {
                session.Dispose();
            }
        }

        public async Task AbortTransactionAsync(IClientSessionHandle session)
        {
            try
            {
                await session.AbortTransactionAsync();
            }
            finally
            {
                session.Dispose();
            }
        }
    }
}
