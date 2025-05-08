using Microsoft.EntityFrameworkCore;
using ReviewsService.Domain.Models;
using ReviewsService.Infrastructure.Data;

namespace ReviewsService.Infrastructure.Repositories;

public class ReviewRepository(ReviewDbContext context) : IReviewRepository
{
    public async Task<IEnumerable<Review>> GetAllReviewsAsync()
    {
        return await context.Reviews.ToListAsync();
    }

    public async Task<Review?> GetReviewByIdAsync(Guid id)
    {
        return await context.Reviews.FindAsync(id);
    }

    public async Task<IEnumerable<Review>> GetReviewsByReviewerIdAsync(Guid reviewerId)
    {
        return await context.Reviews
            .Where(r => r.ReviewerId == reviewerId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewsByTargetIdAsync(Guid targetId, string targetType)
    {
        return await context.Reviews
            .Where(r => r.TargetId == targetId && r.TargetType == targetType)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<double> GetAverageRatingForTargetAsync(Guid targetId, string targetType)
    {
        return await context.Reviews
            .Where(r => r.TargetId == targetId && r.TargetType == targetType)
            .AverageAsync(r => r.Rating);
    }

    public async Task<Review> CreateReviewAsync(Review review)
    {
        review.CreatedAt = DateTime.UtcNow;
        review.UpdatedAt = DateTime.UtcNow;

        context.Reviews.Add(review);
        await context.SaveChangesAsync();
        return review;
    }

    public async Task UpdateReviewAsync(Review review)
    {
        var existingReview = await context.Reviews.FindAsync(review.Id);
        if (existingReview != null)
        {
            review.UpdatedAt = DateTime.UtcNow;
            // Preserve the original creation date and helpful count
            review.CreatedAt = existingReview.CreatedAt;
            review.Helpful = existingReview.Helpful;

            context.Entry(existingReview).CurrentValues.SetValues(review);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteReviewAsync(Guid id)
    {
        var review = await context.Reviews.FindAsync(id);
        if (review != null)
        {
            context.Reviews.Remove(review);
            await context.SaveChangesAsync();
        }
    }

    public async Task IncrementHelpfulCountAsync(Guid id)
    {
        var review = await context.Reviews.FindAsync(id);
        if (review != null)
        {
            review.Helpful += 1;
            await context.SaveChangesAsync();
        }
    }
}
