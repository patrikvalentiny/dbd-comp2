namespace listings_service.CQRS.Commands
{
    public interface IUpdateListingStatusCommand
    {
        Task<bool> ExecuteAsync(string id, string status);
    }
}
