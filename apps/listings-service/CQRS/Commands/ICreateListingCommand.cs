using listings_service.Models;

namespace listings_service.CQRS.Commands
{
    public interface ICreateListingCommand
    {
        Task<Listing> ExecuteAsync(Listing listing);
    }
}
