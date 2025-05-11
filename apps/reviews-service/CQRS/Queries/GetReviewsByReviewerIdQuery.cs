using ReviewsService.Domain.Models;
using ReviewsService.Infrastructure.Repositories;

namespace ReviewsService.CQRS.Queries
{
    public class GetReviewsByReviewerIdQuery : IGetReviewsByReviewerIdQuery
    {
        private readonly IReviewRepository _reviewRepository;

        public GetReviewsByReviewerIdQuery(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<Review>> ExecuteAsync(Guid reviewerId)
        {
            return await _reviewRepository.GetReviewsByReviewerIdAsync(reviewerId);
        }
    }
}
