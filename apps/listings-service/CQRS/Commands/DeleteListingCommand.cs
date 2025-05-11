using listings_service.Services;

namespace listings_service.CQRS.Commands
{
    public class DeleteListingCommand : IDeleteListingCommand
    {
        private readonly ListingService _listingService;

        public DeleteListingCommand(ListingService listingService)
        {
            _listingService = listingService;
        }

        public async Task<bool> ExecuteAsync(string id)
        {
            return await _listingService.DeleteListingAsync(id);
        }
    }
}
