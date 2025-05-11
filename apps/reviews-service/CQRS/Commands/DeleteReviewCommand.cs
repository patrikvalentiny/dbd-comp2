using ReviewsService.Infrastructure.Repositories;

namespace ReviewsService.CQRS.Commands
{
    public class DeleteReviewCommand : IDeleteReviewCommand
    {
        private readonly IReviewRepository _reviewRepository;

        public DeleteReviewCommand(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task ExecuteAsync(Guid id)
        {
            await _reviewRepository.DeleteReviewAsync(id);
        }
    }
}
