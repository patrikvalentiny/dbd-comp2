using Microsoft.EntityFrameworkCore;
using OrdersService.Domain.Models;
using OrdersService.Infrastructure.Data;

namespace OrdersService.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext context) : IOrderRepository
    {
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await context.Orders
                .Include(o => o.ShippingInfo)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            return await context.Orders
                .Include(o => o.ShippingInfo)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByBuyerIdAsync(Guid buyerId)
        {
            return await context.Orders
                .Include(o => o.ShippingInfo)
                .Where(o => o.BuyerId == buyerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersBySellerIdAsync(Guid sellerId)
        {
            return await context.Orders
                .Include(o => o.ShippingInfo)
                .Where(o => o.SellerId == sellerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByListingIdAsync(string listingId)
        {
            return await context.Orders
                .Include(o => o.ShippingInfo)
                .Where(o => o.ListingId == listingId)
                .ToListAsync();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
            
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            return order;
        }

        public async Task UpdateOrderAsync(Order order)
        {
            var existingOrder = await context.Orders.FindAsync(order.Id);
            if (existingOrder != null)
            {
                order.UpdatedAt = DateTime.UtcNow;
                context.Entry(existingOrder).CurrentValues.SetValues(order);
                
                // Handle shipping info separately
                if (order.ShippingInfo != null)
                {
                    var existingShipping = await context.ShippingInfos.FirstOrDefaultAsync(s => s.OrderId == order.Id);
                    if (existingShipping != null)
                    {
                        context.Entry(existingShipping).CurrentValues.SetValues(order.ShippingInfo);
                    }
                    else
                    {
                        order.ShippingInfo.OrderId = order.Id;
                        context.ShippingInfos.Add(order.ShippingInfo);
                    }
                }
                
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            var order = await context.Orders.FindAsync(id);
            if (order != null)
            {
                context.Orders.Remove(order);
                await context.SaveChangesAsync();
            }
        }
    }
}
