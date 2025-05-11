namespace listings_service.CQRS.Commands
{
    public interface IDeleteListingCommand
    {
        Task<bool> ExecuteAsync(string id);
    }
}
