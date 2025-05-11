using OrdersService.Infrastructure.Repositories;

namespace OrdersService.CQRS.Commands
{
    public class UpdateOrderStatusCommand : IUpdateOrderStatusCommand
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderStatusCommand(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task ExecuteAsync(Guid id, string status)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order != null)
            {
                order.Status = status;
                order.UpdatedAt = DateTime.UtcNow;
                await _orderRepository.UpdateOrderAsync(order);
            }
        }
    }
}
