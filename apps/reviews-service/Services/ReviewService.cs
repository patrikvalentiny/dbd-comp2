using ReviewsService.Domain.Models;
using ReviewsService.CQRS.Commands;
using ReviewsService.CQRS.Queries;

namespace ReviewsService.Services;

public class ReviewService : IReviewService
{
    private readonly IGetAllReviewsQuery _getAllReviewsQuery;
    private readonly IGetReviewByIdQuery _getReviewByIdQuery;
    private readonly IGetReviewsByReviewerIdQuery _getReviewsByReviewerIdQuery;
    private readonly IGetReviewsByTargetQuery _getReviewsByTargetQuery;
    private readonly IGetAverageRatingQuery _getAverageRatingQuery;
    private readonly ICreateReviewCommand _createReviewCommand;
    private readonly IUpdateReviewCommand _updateReviewCommand;
    private readonly IDeleteReviewCommand _deleteReviewCommand;
    private readonly IMarkReviewAsHelpfulCommand _markReviewAsHelpfulCommand;

    public ReviewService(
        IGetAllReviewsQuery getAllReviewsQuery,
        IGetReviewByIdQuery getReviewByIdQuery,
        IGetReviewsByReviewerIdQuery getReviewsByReviewerIdQuery,
        IGetReviewsByTargetQuery getReviewsByTargetQuery,
        IGetAverageRatingQuery getAverageRatingQuery,
        ICreateReviewCommand createReviewCommand,
        IUpdateReviewCommand updateReviewCommand,
        IDeleteReviewCommand deleteReviewCommand,
        IMarkReviewAsHelpfulCommand markReviewAsHelpfulCommand)
    {
        _getAllReviewsQuery = getAllReviewsQuery;
        _getReviewByIdQuery = getReviewByIdQuery;
        _getReviewsByReviewerIdQuery = getReviewsByReviewerIdQuery;
        _getReviewsByTargetQuery = getReviewsByTargetQuery;
        _getAverageRatingQuery = getAverageRatingQuery;
        _createReviewCommand = createReviewCommand;
        _updateReviewCommand = updateReviewCommand;
        _deleteReviewCommand = deleteReviewCommand;
        _markReviewAsHelpfulCommand = markReviewAsHelpfulCommand;
    }

    public async Task<IEnumerable<Review>> GetAllReviewsAsync()
    {
        return await _getAllReviewsQuery.ExecuteAsync();
    }

    public async Task<Review?> GetReviewByIdAsync(Guid id)
    {
        return await _getReviewByIdQuery.ExecuteAsync(id);
    }

    public async Task<IEnumerable<Review>> GetReviewsByReviewerIdAsync(Guid reviewerId)
    {
        return await _getReviewsByReviewerIdQuery.ExecuteAsync(reviewerId);
    }

    public async Task<IEnumerable<Review>> GetReviewsBySellerIdAsync(Guid sellerId)
    {
        return await _getReviewsByTargetQuery.ExecuteAsync(sellerId, "seller");
    }

    public async Task<IEnumerable<Review>> GetReviewsByBuyerIdAsync(Guid buyerId)
    {
        return await _getReviewsByTargetQuery.ExecuteAsync(buyerId, "buyer");
    }

    public async Task<IEnumerable<Review>> GetReviewsByItemIdAsync(Guid itemId)
    {
        return await _getReviewsByTargetQuery.ExecuteAsync(itemId, "item");
    }

    public async Task<double> GetSellerRatingAsync(Guid sellerId)
    {
        return await _getAverageRatingQuery.ExecuteAsync(sellerId, "seller");
    }

    public async Task<double> GetBuyerRatingAsync(Guid buyerId)
    {
        return await _getAverageRatingQuery.ExecuteAsync(buyerId, "buyer");
    }

    public async Task<double> GetItemRatingAsync(Guid itemId)
    {
        return await _getAverageRatingQuery.ExecuteAsync(itemId, "item");
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

        return await _createReviewCommand.ExecuteAsync(review);
    }

    public async Task UpdateReviewAsync(Review review)
    {
        if (review.Rating < 1 || review.Rating > 5)
        {
            throw new ArgumentException("Rating must be between 1 and 5");
        }

        await _updateReviewCommand.ExecuteAsync(review);
    }

    public async Task DeleteReviewAsync(Guid id)
    {
        await _deleteReviewCommand.ExecuteAsync(id);
    }

    public async Task MarkReviewAsHelpfulAsync(Guid id)
    {
        await _markReviewAsHelpfulCommand.ExecuteAsync(id);
    }
}
