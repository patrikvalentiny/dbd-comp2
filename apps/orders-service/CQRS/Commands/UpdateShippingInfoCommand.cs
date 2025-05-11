using OrdersService.Domain.Models;
using OrdersService.Infrastructure.Repositories;

namespace OrdersService.CQRS.Commands
{
    public class UpdateShippingInfoCommand : IUpdateShippingInfoCommand
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateShippingInfoCommand(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task ExecuteAsync(Guid orderId, ShippingInfo shippingInfo)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                shippingInfo.OrderId = orderId;
                order.ShippingInfo = shippingInfo;
                order.UpdatedAt = DateTime.UtcNow;
                await _orderRepository.UpdateOrderAsync(order);
            }
        }
    }
}
