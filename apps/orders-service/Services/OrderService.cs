using OrdersService.Domain.Models;
using OrdersService.CQRS.Commands;
using OrdersService.CQRS.Queries;

namespace OrdersService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGetAllOrdersQuery _getAllOrdersQuery;
        private readonly IGetOrderByIdQuery _getOrderByIdQuery;
        private readonly IGetOrdersByBuyerIdQuery _getOrdersByBuyerIdQuery;
        private readonly IGetOrdersBySellerIdQuery _getOrdersBySellerIdQuery;
        private readonly ICreateOrderCommand _createOrderCommand;
        private readonly IUpdateOrderStatusCommand _updateOrderStatusCommand;
        private readonly IUpdateShippingInfoCommand _updateShippingInfoCommand;
        private readonly IDeleteOrderCommand _deleteOrderCommand;

        public OrderService(
            IGetAllOrdersQuery getAllOrdersQuery,
            IGetOrderByIdQuery getOrderByIdQuery,
            IGetOrdersByBuyerIdQuery getOrdersByBuyerIdQuery,
            IGetOrdersBySellerIdQuery getOrdersBySellerIdQuery,
            ICreateOrderCommand createOrderCommand,
            IUpdateOrderStatusCommand updateOrderStatusCommand,
            IUpdateShippingInfoCommand updateShippingInfoCommand,
            IDeleteOrderCommand deleteOrderCommand)
        {
            _getAllOrdersQuery = getAllOrdersQuery;
            _getOrderByIdQuery = getOrderByIdQuery;
            _getOrdersByBuyerIdQuery = getOrdersByBuyerIdQuery;
            _getOrdersBySellerIdQuery = getOrdersBySellerIdQuery;
            _createOrderCommand = createOrderCommand;
            _updateOrderStatusCommand = updateOrderStatusCommand;
            _updateShippingInfoCommand = updateShippingInfoCommand;
            _deleteOrderCommand = deleteOrderCommand;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _getAllOrdersQuery.ExecuteAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            return await _getOrderByIdQuery.ExecuteAsync(id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByBuyerIdAsync(Guid buyerId)
        {
            return await _getOrdersByBuyerIdQuery.ExecuteAsync(buyerId);
        }

        public async Task<IEnumerable<Order>> GetOrdersBySellerIdAsync(Guid sellerId)
        {
            return await _getOrdersBySellerIdQuery.ExecuteAsync(sellerId);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            // Additional business logic can be added here
            return await _createOrderCommand.ExecuteAsync(order);
        }

        public async Task UpdateOrderStatusAsync(Guid id, string status)
        {
            await _updateOrderStatusCommand.ExecuteAsync(id, status);
        }

        public async Task UpdateShippingInfoAsync(Guid orderId, ShippingInfo shippingInfo)
        {
            await _updateShippingInfoCommand.ExecuteAsync(orderId, shippingInfo);
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            await _deleteOrderCommand.ExecuteAsync(id);
        }
    }
}
