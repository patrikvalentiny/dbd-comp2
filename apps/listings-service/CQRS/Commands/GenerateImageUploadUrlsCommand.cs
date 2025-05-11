using listings_service.Services;

namespace listings_service.CQRS.Commands
{
    public class GenerateImageUploadUrlsCommand : IGenerateImageUploadUrlsCommand
    {
        private readonly ListingService _listingService;

        public GenerateImageUploadUrlsCommand(ListingService listingService)
        {
            _listingService = listingService;
        }

        public async Task<List<UploadUrlResponse>> ExecuteAsync(string listingId, List<string> fileNames)
        {
            return await _listingService.GenerateImageUploadUrlsAsync(listingId, fileNames);
        }
    }
}
