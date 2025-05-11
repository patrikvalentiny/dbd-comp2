using listings_service.Models;

namespace listings_service.CQRS.Queries
{
    public interface IGetListingsBySellerIdQuery
    {
        Task<IEnumerable<Listing>> ExecuteAsync(string sellerId);
    }
}
