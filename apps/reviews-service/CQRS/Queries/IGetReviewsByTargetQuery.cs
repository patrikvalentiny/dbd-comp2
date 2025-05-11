using ReviewsService.Domain.Models;

namespace ReviewsService.CQRS.Queries
{
    public interface IGetReviewsByTargetQuery
    {
        Task<IEnumerable<Review>> ExecuteAsync(Guid targetId, string targetType);
    }
}
