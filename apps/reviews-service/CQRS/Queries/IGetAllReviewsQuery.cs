using ReviewsService.Domain.Models;

namespace ReviewsService.CQRS.Queries
{
    public interface IGetAllReviewsQuery
    {
        Task<IEnumerable<Review>> ExecuteAsync();
    }
}
