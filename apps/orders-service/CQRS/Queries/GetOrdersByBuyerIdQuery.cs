using OrdersService.Domain.Models;
using OrdersService.Infrastructure.Repositories;

namespace OrdersService.CQRS.Queries
{
    public class GetOrdersByBuyerIdQuery : IGetOrdersByBuyerIdQuery
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersByBuyerIdQuery(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> ExecuteAsync(Guid buyerId)
        {
            return await _orderRepository.GetOrdersByBuyerIdAsync(buyerId);
        }
    }
}
