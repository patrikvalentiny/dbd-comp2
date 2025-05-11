using OrdersService.Domain.Models;
using OrdersService.Infrastructure.Repositories;

namespace OrdersService.CQRS.Queries
{
    public class GetAllOrdersQuery : IGetAllOrdersQuery
    {
        private readonly IOrderRepository _orderRepository;

        public GetAllOrdersQuery(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> ExecuteAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }
    }
}
