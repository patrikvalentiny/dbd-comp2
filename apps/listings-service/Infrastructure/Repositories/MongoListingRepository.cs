using listings_service.Infrastructure.Contexts;
using listings_service.Models;
using ListingsService.Repositories;
using MongoDB.Driver;

namespace listings_service.Repositories
{
    public class MongoListingRepository(MongoContext context) : IListingRepository
    {
        private readonly IMongoCollection<Listing> _listings = context.Listings;

        public async Task<IEnumerable<Listing>> GetAllAsync()
        {
            return await _listings.Find(listing => listing.Status == "active").ToListAsync();
        }

        public async Task<Listing> GetByIdAsync(string id)
        {
            return await _listings.Find(listing => listing.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Listing>> GetBySellerIdAsync(string sellerId)
        {
            return await _listings.Find(listing => listing.SellerId == sellerId).ToListAsync();
        }

        public async Task<IEnumerable<Listing>> SearchAsync(string query, string category, string condition, decimal? minPrice, decimal? maxPrice)
        {
            var builder = Builders<Listing>.Filter;
            var filter = builder.Where(l => l.Status == "active");

            if (!string.IsNullOrEmpty(query))
            {
                filter &= builder.Text(query);
            }

            if (!string.IsNullOrEmpty(category))
            {
                filter &= builder.Eq(l => l.Category, category);
            }

            if (!string.IsNullOrEmpty(condition))
            {
                filter &= builder.Eq(l => l.Condition, condition);
            }

            if (minPrice.HasValue)
            {
                filter &= builder.Gte(l => l.Price, minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                filter &= builder.Lte(l => l.Price, maxPrice.Value);
            }

            return await _listings.Find(filter).ToListAsync();
        }

        public async Task<Listing> CreateAsync(Listing listing)
        {
            listing.CreatedAt = DateTime.UtcNow;
            listing.UpdatedAt = DateTime.UtcNow;
            await _listings.InsertOneAsync(listing);
            return listing;
        }

        public async Task<bool> UpdateAsync(string id, Listing listingIn)
        {
            listingIn.UpdatedAt = DateTime.UtcNow;
            var result = await _listings.ReplaceOneAsync(listing => listing.Id == id, listingIn);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            // Soft delete by changing status
            var update = Builders<Listing>.Update
                .Set(l => l.Status, "deleted")
                .Set(l => l.UpdatedAt, DateTime.UtcNow);
                
            var result = await _listings.UpdateOneAsync(l => l.Id == id, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> UpdateStatusAsync(string id, string status)
        {
            var update = Builders<Listing>.Update
                .Set(l => l.Status, status)
                .Set(l => l.UpdatedAt, DateTime.UtcNow);
                
            var result = await _listings.UpdateOneAsync(l => l.Id == id, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task IncrementViewCountAsync(string id)
        {
            var update = Builders<Listing>.Update
                .Inc(l => l.Metadata.Views, 1)
                .Set(l => l.UpdatedAt, DateTime.UtcNow);
                
            await _listings.UpdateOneAsync(l => l.Id == id, update);
        }
    }
}
