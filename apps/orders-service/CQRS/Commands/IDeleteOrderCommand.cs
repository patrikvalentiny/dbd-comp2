namespace OrdersService.CQRS.Commands
{
    public interface IDeleteOrderCommand
    {
        Task ExecuteAsync(Guid id);
    }
}
