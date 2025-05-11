using OrdersService.Domain.Models;
using OrdersService.Infrastructure.Repositories;

namespace OrdersService.CQRS.Queries
{
    public class GetOrdersBySellerIdQuery : IGetOrdersBySellerIdQuery
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersBySellerIdQuery(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> ExecuteAsync(Guid sellerId)
        {
            return await _orderRepository.GetOrdersBySellerIdAsync(sellerId);
        }
    }
}
