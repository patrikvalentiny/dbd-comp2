using OrdersService.Domain.Models;

namespace OrdersService.CQRS.Queries
{
    public interface IGetOrdersByBuyerIdQuery
    {
        Task<IEnumerable<Order>> ExecuteAsync(Guid buyerId);
    }
}
