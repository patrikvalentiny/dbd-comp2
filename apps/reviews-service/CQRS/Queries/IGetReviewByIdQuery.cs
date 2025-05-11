using ReviewsService.Domain.Models;

namespace ReviewsService.CQRS.Queries
{
    public interface IGetReviewByIdQuery
    {
        Task<Review?> ExecuteAsync(Guid id);
    }
}
