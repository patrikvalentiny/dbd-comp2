using ReviewsService.Domain.Models;

namespace ReviewsService.CQRS.Queries
{
    public interface IGetReviewsByReviewerIdQuery
    {
        Task<IEnumerable<Review>> ExecuteAsync(Guid reviewerId);
    }
}
