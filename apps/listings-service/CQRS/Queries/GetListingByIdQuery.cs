using listings_service.Models;
using listings_service.Services;

namespace listings_service.CQRS.Queries
{
    public class GetListingByIdQuery : IGetListingByIdQuery
    {
        private readonly ListingService _listingService;

        public GetListingByIdQuery(ListingService listingService)
        {
            _listingService = listingService;
        }

        public async Task<Listing?> ExecuteAsync(string id)
        {
            return await _listingService.GetListingByIdAsync(id);
        }
    }
}
