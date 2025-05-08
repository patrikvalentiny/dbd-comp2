using ReviewsService.Domain.Models;
using ReviewsService.Infrastructure.Repositories;

namespace ReviewsService.Services;
public class ReviewService(IReviewRepository reviewRepository) : IReviewService
{
    public async Task<IEnumerable<Review>> GetAllReviewsAsync()
    {
        return await reviewRepository.GetAllReviewsAsync();
    }

    public async Task<Review?> GetReviewByIdAsync(Guid id)
    {
        return await reviewRepository.GetReviewByIdAsync(id);
    }

    public async Task<IEnumerable<Review>> GetReviewsByReviewerIdAsync(Guid reviewerId)
    {
        return await reviewRepository.GetReviewsByReviewerIdAsync(reviewerId);
    }

    public async Task<IEnumerable<Review>> GetReviewsBySellerIdAsync(Guid sellerId)
    {
        return await reviewRepository.GetReviewsByTargetIdAsync(sellerId, "seller");
    }

    public async Task<IEnumerable<Review>> GetReviewsByBuyerIdAsync(Guid buyerId)
    {
        return await reviewRepository.GetReviewsByTargetIdAsync(buyerId, "buyer");
    }

    public async Task<IEnumerable<Review>> GetReviewsByItemIdAsync(Guid itemId)
    {
        return await reviewRepository.GetReviewsByTargetIdAsync(itemId, "item");
    }

    public async Task<double> GetSellerRatingAsync(Guid sellerId)
    {
        return await reviewRepository.GetAverageRatingForTargetAsync(sellerId, "seller");
    }

    public async Task<double> GetBuyerRatingAsync(Guid buyerId)
    {
        return await reviewRepository.GetAverageRatingForTargetAsync(buyerId, "buyer");
    }

    public async Task<double> GetItemRatingAsync(Guid itemId)
    {
        return await reviewRepository.GetAverageRatingForTargetAsync(itemId, "item");
    }

    public async Task<Review> CreateReviewAsync(Review review)
    {
        // Add any business validation here
        if (review.Rating < 1 || review.Rating > 5)
        {
            throw new ArgumentException("Rating must be between 1 and 5");
        }

        // Ensure valid target type
        if (!(new[] { "seller", "buyer", "item" }).Contains(review.TargetType))
        {
            throw new ArgumentException("Target type must be seller, buyer, or item");
        }

        return await reviewRepository.CreateReviewAsync(review);
    }

    public async Task UpdateReviewAsync(Review review)
    {
        if (review.Rating < 1 || review.Rating > 5)
        {
            throw new ArgumentException("Rating must be between 1 and 5");
        }

        await reviewRepository.UpdateReviewAsync(review);
    }

    public async Task DeleteReviewAsync(Guid id)
    {
        await reviewRepository.DeleteReviewAsync(id);
    }

    public async Task MarkReviewAsHelpfulAsync(Guid id)
    {
        await reviewRepository.IncrementHelpfulCountAsync(id);
    }
}
