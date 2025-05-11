using listings_service.Models;
using listings_service.Services;

namespace listings_service.CQRS.Commands
{
    public class UpdateListingCommand : IUpdateListingCommand
    {
        private readonly ListingService _listingService;

        public UpdateListingCommand(ListingService listingService)
        {
            _listingService = listingService;
        }

        public async Task<bool> ExecuteAsync(string id, Listing listing)
        {
            return await _listingService.UpdateListingAsync(id, listing);
        }
    }
}
