using MessageClient.Interfaces;
using Messages;
using OrdersService.Infrastructure.Repositories;

namespace OrdersService.MessageHandlers;

public class UserDeletedHandler : IMessageHandler<UserDeleted>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<UserDeletedHandler> _logger;

    public UserDeletedHandler(IOrderRepository orderRepository, ILogger<UserDeletedHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task Handler(UserDeleted message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling user deletion for user {UserId}", message.UserId);

        // Find orders where the user is a buyer
        var buyerOrders = await _orderRepository.GetOrdersByBuyerIdAsync(message.UserId);

        // Find orders where the user is a seller
        var sellerOrders = await _orderRepository.GetOrdersBySellerIdAsync(message.UserId);

        foreach (var order in buyerOrders)
        {
            // For completed orders, anonymize user data
            if (order.Status == "delivered" || order.Status == "completed")
            {
                order.BuyerId = Guid.Empty; // Or use a special "deleted" placeholder
                await _orderRepository.UpdateOrderAsync(order);
                _logger.LogInformation("Anonymized buyer in order {OrderId} for deleted user {UserId}",
                    order.Id, message.UserId);
            }
            else
            {
                // For active orders, cancel them
                order.Status = "cancelled";
                order.UpdatedAt = DateTime.UtcNow;
                await _orderRepository.UpdateOrderAsync(order);
                _logger.LogInformation("Cancelled order {OrderId} for deleted buyer {UserId}",
                    order.Id, message.UserId);
            }
        }

        // Similar handling for seller orders
        foreach (var order in sellerOrders)
        {
            // For completed orders, anonymize user data
            if (order.Status == "delivered" || order.Status == "completed")
            {
                order.SellerId = Guid.Empty; // Or use a special "deleted" placeholder
                await _orderRepository.UpdateOrderAsync(order);
                _logger.LogInformation("Anonymized seller in order {OrderId} for deleted user {UserId}",
                    order.Id, message.UserId);
            }
            else
            {
                // For active orders, cancel them
                order.Status = "cancelled";
                order.UpdatedAt = DateTime.UtcNow;
                await _orderRepository.UpdateOrderAsync(order);
                _logger.LogInformation("Cancelled order {OrderId} for deleted seller {UserId}",
                    order.Id, message.UserId);
            }
        }
    }
}
