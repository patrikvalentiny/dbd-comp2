using ReviewsService.Infrastructure.Repositories;

namespace ReviewsService.CQRS.Commands
{
    public class MarkReviewAsHelpfulCommand : IMarkReviewAsHelpfulCommand
    {
        private readonly IReviewRepository _reviewRepository;

        public MarkReviewAsHelpfulCommand(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task ExecuteAsync(Guid id)
        {
            await _reviewRepository.IncrementHelpfulCountAsync(id);
        }
    }
}
