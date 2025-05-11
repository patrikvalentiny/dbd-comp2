using ReviewsService.Infrastructure.Repositories;

namespace ReviewsService.CQRS.Queries
{
    public class GetAverageRatingQuery : IGetAverageRatingQuery
    {
        private readonly IReviewRepository _reviewRepository;

        public GetAverageRatingQuery(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<double> ExecuteAsync(Guid targetId, string targetType)
        {
            return await _reviewRepository.GetAverageRatingForTargetAsync(targetId, targetType);
        }
    }
}
