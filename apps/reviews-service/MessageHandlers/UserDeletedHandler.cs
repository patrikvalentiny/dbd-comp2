using MessageClient.Interfaces;
using Messages;
using ReviewsService.Infrastructure.Repositories;

namespace ReviewsService.MessageHandlers;

public class UserDeletedHandler : IMessageHandler<UserDeleted>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ILogger<UserDeletedHandler> _logger;

    public UserDeletedHandler(IReviewRepository reviewRepository, ILogger<UserDeletedHandler> logger)
    {
        _reviewRepository = reviewRepository;
        _logger = logger;
    }

    public async Task Handler(UserDeleted message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling user deletion for user {UserId}", message.UserId);

        // Find reviews written by the deleted user
        var writtenReviews = await _reviewRepository.GetReviewsByReviewerIdAsync(message.UserId);

        // Find reviews about the deleted user
        var reviewsAboutUser = await _reviewRepository.GetReviewsByTargetIdAsync(message.UserId, "seller");

        // Handle reviews written by this user
        foreach (var review in writtenReviews)
        {
            // Option 1: Delete the reviews
            // await _reviewRepository.DeleteReviewAsync(review.Id);

            // Option 2: Anonymize the reviewer
            review.ReviewerId = Guid.Empty; // Or some "deleted_user" placeholder
            await _reviewRepository.UpdateReviewAsync(review);
            _logger.LogInformation("Anonymized reviewer in review {ReviewId} for deleted user {UserId}",
                review.Id, message.UserId);
        }

        // Handle reviews about this user
        foreach (var review in reviewsAboutUser)
        {
            // Either delete or keep as historical data with anonymized target
            review.TargetId = Guid.Empty; // Or some "deleted_user" placeholder
            await _reviewRepository.UpdateReviewAsync(review);
            _logger.LogInformation("Anonymized target in review {ReviewId} for deleted user {UserId}",
                review.Id, message.UserId);
        }
    }
}
