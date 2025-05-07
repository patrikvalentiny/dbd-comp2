using listings_service.Models;

namespace ListingsService.Repositories
{
    public interface IListingRepository
    {
        Task<IEnumerable<Listing>> GetAllAsync();
        Task<Listing> GetByIdAsync(string id);
        Task<IEnumerable<Listing>> GetBySellerIdAsync(string sellerId);
        Task<IEnumerable<Listing>> SearchAsync(string query, string category, string condition, decimal? minPrice, decimal? maxPrice);
        Task<Listing> CreateAsync(Listing listing);
        Task<bool> UpdateAsync(string id, Listing listingIn);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateStatusAsync(string id, string status);
        Task IncrementViewCountAsync(string id);
    }
}
