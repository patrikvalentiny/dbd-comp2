using ReviewsService.Domain.Models;
using ReviewsService.Infrastructure.Repositories;

namespace ReviewsService.CQRS.Queries
{
    public class GetReviewsByTargetQuery : IGetReviewsByTargetQuery
    {
        private readonly IReviewRepository _reviewRepository;

        public GetReviewsByTargetQuery(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<Review>> ExecuteAsync(Guid targetId, string targetType)
        {
            return await _reviewRepository.GetReviewsByTargetIdAsync(targetId, targetType);
        }
    }
}
