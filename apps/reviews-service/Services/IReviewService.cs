using ReviewsService.Domain.Models;

namespace ReviewsService.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review?> GetReviewByIdAsync(Guid id);
        Task<IEnumerable<Review>> GetReviewsByReviewerIdAsync(Guid reviewerId);
        Task<IEnumerable<Review>> GetReviewsBySellerIdAsync(Guid sellerId);
        Task<IEnumerable<Review>> GetReviewsByBuyerIdAsync(Guid buyerId);
        Task<IEnumerable<Review>> GetReviewsByItemIdAsync(Guid itemId);
        Task<double> GetSellerRatingAsync(Guid sellerId);
        Task<double> GetBuyerRatingAsync(Guid buyerId);
        Task<double> GetItemRatingAsync(Guid itemId);
        Task<Review> CreateReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(Guid id);
        Task MarkReviewAsHelpfulAsync(Guid id);
    }
}
