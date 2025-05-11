using ReviewsService.Domain.Models;
using ReviewsService.Infrastructure.Repositories;

namespace ReviewsService.CQRS.Queries
{
    public class GetReviewByIdQuery : IGetReviewByIdQuery
    {
        private readonly IReviewRepository _reviewRepository;

        public GetReviewByIdQuery(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Review?> ExecuteAsync(Guid id)
        {
            return await _reviewRepository.GetReviewByIdAsync(id);
        }
    }
}
