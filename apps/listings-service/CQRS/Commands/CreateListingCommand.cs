using listings_service.Models;
using listings_service.Services;

namespace listings_service.CQRS.Commands
{
    public class CreateListingCommand : ICreateListingCommand
    {
        private readonly ListingService _listingService;

        public CreateListingCommand(ListingService listingService)
        {
            _listingService = listingService;
        }

        public async Task<Listing> ExecuteAsync(Listing listing)
        {
            return await _listingService.CreateListingAsync(listing);
        }
    }
}
