using listings_service.Models;

namespace listings_service.CQRS.Queries
{
    public interface IGetListingByIdQuery
    {
        Task<Listing?> ExecuteAsync(string id);
    }
}
