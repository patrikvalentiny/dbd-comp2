using ReviewsService.Domain.Models;
using ReviewsService.Infrastructure.Repositories;

namespace ReviewsService.CQRS.Queries
{
    public class GetAllReviewsQuery : IGetAllReviewsQuery
    {
        private readonly IReviewRepository _reviewRepository;

        public GetAllReviewsQuery(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<Review>> ExecuteAsync()
        {
            return await _reviewRepository.GetAllReviewsAsync();
        }
    }
}
