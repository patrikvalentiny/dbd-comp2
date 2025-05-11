using listings_service.Models;

namespace listings_service.CQRS.Queries
{
    public interface ISearchListingsQuery
    {
        Task<IEnumerable<Listing>> ExecuteAsync(string query, string category, string condition, decimal? minPrice, decimal? maxPrice);
    }
}
