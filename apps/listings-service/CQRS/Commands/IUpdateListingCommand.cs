using listings_service.Models;

namespace listings_service.CQRS.Commands
{
    public interface IUpdateListingCommand
    {
        Task<bool> ExecuteAsync(string id, Listing listing);
    }
}
