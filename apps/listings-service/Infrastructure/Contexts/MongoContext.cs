using listings_service.Models;
using MongoDB.Driver;

namespace listings_service.Infrastructure.Contexts
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;
        private readonly MongoClient _client;

        public MongoContext(IConfiguration configuration)
        {
            // Use credentials from compose file - prioritize configuration but fallback to defaults
            var user = configuration["MongoDB:User"] ?? "mongodb";
            var password = configuration["MongoDB:Password"] ?? "mongodb";
            var host = configuration["MongoDB:Host"] ?? "localhost";
            var port = configuration["MongoDB:Port"] ?? "27017";
            var databaseName = configuration["MongoDB:DatabaseName"] ?? "secondhand_marketplace";

            var connectionString = $"mongodb://{user}:{password}@{host}:{port}";

            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
            
            // Initialize collections and indexes
            InitializeCollections();
        }
        public IMongoClient Client => _client;
        public IMongoCollection<Listing> Listings => _database.GetCollection<Listing>("listings");

        private void InitializeCollections()
        {
            // Create listing indexes
            var indexKeysBuilder = Builders<Listing>.IndexKeys;
            
            // Text search index
            var textIndexModel = new CreateIndexModel<Listing>(
                indexKeysBuilder.Text(l => l.Title).Text(l => l.Description));
            
            
            // Category + price index for filtering
            var categoryPriceIndexModel = new CreateIndexModel<Listing>(
                indexKeysBuilder.Ascending(l => l.Category).Ascending(l => l.Price));
            
            // Status index
            var statusIndexModel = new CreateIndexModel<Listing>(
                indexKeysBuilder.Ascending(l => l.Status));
            
            Listings.Indexes.CreateMany([textIndexModel, categoryPriceIndexModel, statusIndexModel]);
        }
    }
}
