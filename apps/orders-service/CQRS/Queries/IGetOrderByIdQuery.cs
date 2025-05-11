using OrdersService.Domain.Models;

namespace OrdersService.CQRS.Queries
{
    public interface IGetOrderByIdQuery
    {
        Task<Order?> ExecuteAsync(Guid id);
    }
}
