using ReviewsService.Domain.Models;

namespace ReviewsService.CQRS.Commands
{
    public interface ICreateReviewCommand
    {
        Task<Review> ExecuteAsync(Review review);
    }
}
