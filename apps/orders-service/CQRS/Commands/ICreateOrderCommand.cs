using OrdersService.Domain.Models;

namespace OrdersService.CQRS.Commands
{
    public interface ICreateOrderCommand
    {
        Task<Order> ExecuteAsync(Order order);
    }
}
