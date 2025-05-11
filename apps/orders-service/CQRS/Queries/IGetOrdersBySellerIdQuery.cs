using OrdersService.Domain.Models;

namespace OrdersService.CQRS.Queries
{
    public interface IGetOrdersBySellerIdQuery
    {
        Task<IEnumerable<Order>> ExecuteAsync(Guid sellerId);
    }
}
