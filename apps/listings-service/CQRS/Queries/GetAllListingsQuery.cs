using listings_service.Models;
using listings_service.Services;

namespace listings_service.CQRS.Queries
{
    public class GetAllListingsQuery : IGetAllListingsQuery
    {
        private readonly ListingService _listingService;

        public GetAllListingsQuery(ListingService listingService)
        {
            _listingService = listingService;
        }

        public async Task<IEnumerable<Listing>> ExecuteAsync()
        {
            return await _listingService.GetAllListingsAsync();
        }
    }
}
