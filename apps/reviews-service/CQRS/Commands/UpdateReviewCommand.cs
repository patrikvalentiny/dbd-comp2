using ReviewsService.Domain.Models;
using ReviewsService.Infrastructure.Repositories;

namespace ReviewsService.CQRS.Commands
{
    public class UpdateReviewCommand : IUpdateReviewCommand
    {
        private readonly IReviewRepository _reviewRepository;

        public UpdateReviewCommand(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task ExecuteAsync(Review review)
        {
            await _reviewRepository.UpdateReviewAsync(review);
        }
    }
}
