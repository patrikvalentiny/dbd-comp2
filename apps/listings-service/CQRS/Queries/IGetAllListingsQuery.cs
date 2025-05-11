using listings_service.Models;

namespace listings_service.CQRS.Queries
{
    public interface IGetAllListingsQuery
    {
        Task<IEnumerable<Listing>> ExecuteAsync();
    }
}
