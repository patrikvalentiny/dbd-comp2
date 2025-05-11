using listings_service.Models;
using listings_service.Services;

namespace listings_service.CQRS.Queries
{
    public class SearchListingsQuery : ISearchListingsQuery
    {
        private readonly ListingService _listingService;

        public SearchListingsQuery(ListingService listingService)
        {
            _listingService = listingService;
        }

        public async Task<IEnumerable<Listing>> ExecuteAsync(string query, string category, string condition, decimal? minPrice, decimal? maxPrice)
        {
            return await _listingService.SearchListingsAsync(query, category, condition, minPrice, maxPrice);
        }
    }
}
