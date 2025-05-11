using listings_service.Services;

namespace listings_service.CQRS.Commands
{
    public class UpdateListingStatusCommand : IUpdateListingStatusCommand
    {
        private readonly ListingService _listingService;

        public UpdateListingStatusCommand(ListingService listingService)
        {
            _listingService = listingService;
        }

        public async Task<bool> ExecuteAsync(string id, string status)
        {
            return await _listingService.UpdateListingStatusAsync(id, status);
        }
    }
}
