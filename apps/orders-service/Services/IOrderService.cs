using OrdersService.Domain.Models;

namespace OrdersService.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetOrdersByBuyerIdAsync(Guid buyerId);
        Task<IEnumerable<Order>> GetOrdersBySellerIdAsync(Guid sellerId);
        Task<Order> CreateOrderAsync(Order order);
        Task UpdateOrderStatusAsync(Guid id, string status);
        Task UpdateShippingInfoAsync(Guid orderId, ShippingInfo shippingInfo);
        Task DeleteOrderAsync(Guid id);
    }
}
