using ReviewsService.Domain.Models;

namespace ReviewsService.Infrastructure.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review?> GetReviewByIdAsync(Guid id);
        Task<IEnumerable<Review>> GetReviewsByReviewerIdAsync(Guid reviewerId);
        Task<IEnumerable<Review>> GetReviewsByTargetIdAsync(Guid targetId, string targetType);
        Task<double> GetAverageRatingForTargetAsync(Guid targetId, string targetType);
        Task<Review> CreateReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(Guid id);
        Task IncrementHelpfulCountAsync(Guid id);
    }
}
