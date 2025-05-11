using listings_service.Services;

namespace listings_service.CQRS.Commands
{
    public interface IGenerateImageUploadUrlsCommand
    {
        Task<List<UploadUrlResponse>> ExecuteAsync(string listingId, List<string> fileNames);
    }
}
