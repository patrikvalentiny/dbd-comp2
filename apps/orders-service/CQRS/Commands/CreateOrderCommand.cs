using OrdersService.Domain.Models;
using OrdersService.Infrastructure.Repositories;

namespace OrdersService.CQRS.Commands
{
    public class CreateOrderCommand : ICreateOrderCommand
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderCommand(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> ExecuteAsync(Order order)
        {
            return await _orderRepository.CreateOrderAsync(order);
        }
    }
}
