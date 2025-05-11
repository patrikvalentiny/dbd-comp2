using OrdersService.Domain.Models;

namespace OrdersService.CQRS.Queries
{
    public interface IGetAllOrdersQuery
    {
        Task<IEnumerable<Order>> ExecuteAsync();
    }
}
