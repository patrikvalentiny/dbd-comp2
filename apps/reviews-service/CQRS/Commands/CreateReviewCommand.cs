using ReviewsService.Domain.Models;
using ReviewsService.Infrastructure.Repositories;

namespace ReviewsService.CQRS.Commands
{
    public class CreateReviewCommand : ICreateReviewCommand
    {
        private readonly IReviewRepository _reviewRepository;

        public CreateReviewCommand(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Review> ExecuteAsync(Review review)
        {
            return await _reviewRepository.CreateReviewAsync(review);
        }
    }
}
