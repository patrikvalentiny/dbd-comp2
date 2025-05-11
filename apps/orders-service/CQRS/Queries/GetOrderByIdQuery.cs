using OrdersService.Domain.Models;
using OrdersService.Infrastructure.Repositories;

namespace OrdersService.CQRS.Queries
{
    public class GetOrderByIdQuery : IGetOrderByIdQuery
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderByIdQuery(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order?> ExecuteAsync(Guid id)
        {
            return await _orderRepository.GetOrderByIdAsync(id);
        }
    }
}
