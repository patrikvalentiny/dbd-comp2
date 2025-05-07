using listings_service.Models;
using ListingsService.Repositories;
using MongoDB.Bson;

namespace listings_service.Services
{
    public class ListingService(IListingRepository listingRepository)
    {
        public async Task<IEnumerable<Listing>> GetAllListingsAsync()
        {
            return await listingRepository.GetAllAsync();
        }

        public async Task<Listing> GetListingByIdAsync(string id)
        {
            var listing = await listingRepository.GetByIdAsync(id);
            if (listing != null)
            {
                await listingRepository.IncrementViewCountAsync(id);
            }
            return listing;
        }

        public async Task<IEnumerable<Listing>> GetListingsBySellerIdAsync(string sellerId)
        {
            return await listingRepository.GetBySellerIdAsync(sellerId);
        }

        public async Task<IEnumerable<Listing>> SearchListingsAsync(string query, string category, string condition, decimal? minPrice, decimal? maxPrice)
        {
            return await listingRepository.SearchAsync(query, category, condition, minPrice, maxPrice);
        }

        public async Task<Listing> CreateListingAsync(Listing listing)
        {
            listing.Id = ObjectId.GenerateNewId().ToString();
            return await listingRepository.CreateAsync(listing);
        }

        public async Task<bool> UpdateListingAsync(string id, Listing listing)
        {
            var existingListing = await listingRepository.GetByIdAsync(id);
            if (existingListing == null)
                return false;

            listing.Id = id;
            listing.CreatedAt = existingListing.CreatedAt;
            listing.Metadata.Views = existingListing.Metadata.Views;
            
            return await listingRepository.UpdateAsync(id, listing);
        }

        public async Task<bool> DeleteListingAsync(string id)
        {
            return await listingRepository.DeleteAsync(id);
        }

        public async Task<bool> UpdateListingStatusAsync(string id, string status)
        {
            return await listingRepository.UpdateStatusAsync(id, status);
        }
    }
}
