using ReviewsService.Domain.Models;

namespace ReviewsService.CQRS.Commands
{
    public interface IUpdateReviewCommand
    {
        Task ExecuteAsync(Review review);
    }
}
