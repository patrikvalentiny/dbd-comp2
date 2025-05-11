using listings_service.Models;
using listings_service.Services;

namespace listings_service.CQRS.Queries
{
    public class GetListingsBySellerIdQuery : IGetListingsBySellerIdQuery
    {
        private readonly ListingService _listingService;

        public GetListingsBySellerIdQuery(ListingService listingService)
        {
            _listingService = listingService;
        }

        public async Task<IEnumerable<Listing>> ExecuteAsync(string sellerId)
        {
            return await _listingService.GetListingsBySellerIdAsync(sellerId);
        }
    }
}
