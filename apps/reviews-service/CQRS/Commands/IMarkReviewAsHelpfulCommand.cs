namespace ReviewsService.CQRS.Commands
{
    public interface IMarkReviewAsHelpfulCommand
    {
        Task ExecuteAsync(Guid id);
    }
}
