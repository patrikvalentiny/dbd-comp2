namespace ReviewsService.CQRS.Commands
{
    public interface IDeleteReviewCommand
    {
        Task ExecuteAsync(Guid id);
    }
}
