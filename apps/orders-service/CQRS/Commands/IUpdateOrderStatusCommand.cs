namespace OrdersService.CQRS.Commands
{
    public interface IUpdateOrderStatusCommand
    {
        Task ExecuteAsync(Guid id, string status);
    }
}
