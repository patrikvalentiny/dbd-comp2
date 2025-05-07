using OrdersService.Domain.Models;

namespace OrdersService.Infrastructure.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetOrdersByBuyerIdAsync(Guid buyerId);
        Task<IEnumerable<Order>> GetOrdersBySellerIdAsync(Guid sellerId);
        Task<IEnumerable<Order>> GetOrdersByListingIdAsync(string listingId);
        Task<Order> CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(Guid id);
    }
}
