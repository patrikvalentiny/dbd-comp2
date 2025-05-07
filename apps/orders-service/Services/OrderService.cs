using OrdersService.Domain.Models;
using OrdersService.Infrastructure.Repositories;

namespace OrdersService.Services
{
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await orderRepository.GetAllOrdersAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            return await orderRepository.GetOrderByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByBuyerIdAsync(Guid buyerId)
        {
            return await orderRepository.GetOrdersByBuyerIdAsync(buyerId);
        }

        public async Task<IEnumerable<Order>> GetOrdersBySellerIdAsync(Guid sellerId)
        {
            return await orderRepository.GetOrdersBySellerIdAsync(sellerId);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            // Additional business logic can be added here
            return await orderRepository.CreateOrderAsync(order);
        }

        public async Task UpdateOrderStatusAsync(Guid id, string status)
        {
            var order = await orderRepository.GetOrderByIdAsync(id);
            if (order != null)
            {
                order.Status = status;
                order.UpdatedAt = DateTime.UtcNow;
                await orderRepository.UpdateOrderAsync(order);
            }
        }

        public async Task UpdateShippingInfoAsync(Guid orderId, ShippingInfo shippingInfo)
        {
            var order = await orderRepository.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                shippingInfo.OrderId = orderId;
                order.ShippingInfo = shippingInfo;
                order.UpdatedAt = DateTime.UtcNow;
                await orderRepository.UpdateOrderAsync(order);
            }
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            await orderRepository.DeleteOrderAsync(id);
        }
    }
}
