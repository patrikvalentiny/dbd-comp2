using Microsoft.AspNetCore.Mvc;
using OrdersService.Domain.Models;
using OrdersService.Services;

namespace OrdersService.Controllers
{
    /// <summary>
    /// API controller for managing orders in the second-hand e-commerce platform.
    /// Provides endpoints for creating, retrieving, updating, and deleting orders,
    /// as well as managing order status and shipping information.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all orders in the system.
        /// </summary>
        /// <returns>A collection of all orders with their associated shipping information.</returns>
        /// <response code="200">Returns the list of orders</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EndpointName("GetAllOrders")]
        [EndpointSummary("Retrieves all orders in the system.")]
        [EndpointDescription("Returns a collection of all orders with complete details including shipping information.")]
        
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Retrieves a specific order by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the order to retrieve.</param>
        /// <returns>The order with the specified ID and its associated shipping information.</returns>
        /// <response code="200">Returns the requested order</response>
        /// <response code="404">If the order with the specified ID doesn't exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointName("GetOrderById")]
        [EndpointSummary("Retrieves a specific order by ID.")]
        [EndpointDescription("Fetches the complete details of a single order specified by its unique identifier.")]
        
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var order = await orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        /// <summary>
        /// Retrieves all orders placed by a specific buyer.
        /// </summary>
        /// <param name="buyerId">The unique identifier of the buyer.</param>
        /// <returns>A collection of orders placed by the specified buyer.</returns>
        /// <response code="200">Returns the list of orders for the specified buyer</response>
        [HttpGet("buyer/{buyerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EndpointName("GetOrdersByBuyer")]
        [EndpointSummary("Retrieves all orders for a specific buyer.")]
        [EndpointDescription("Returns a collection of orders that were placed by the buyer with the specified ID.")]
        
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByBuyer(Guid buyerId)
        {
            var orders = await orderService.GetOrdersByBuyerIdAsync(buyerId);
            return Ok(orders);
        }

        /// <summary>
        /// Retrieves all orders for a specific seller.
        /// </summary>
        /// <param name="sellerId">The unique identifier of the seller.</param>
        /// <returns>A collection of orders for the specified seller.</returns>
        /// <response code="200">Returns the list of orders for the specified seller</response>
        [HttpGet("seller/{sellerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EndpointName("GetOrdersBySeller")]
        [EndpointSummary("Retrieves all orders for a specific seller.")]
        [EndpointDescription("Returns a collection of orders that were sold by the seller with the specified ID.")]
        
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersBySeller(Guid sellerId)
        {
            var orders = await orderService.GetOrdersBySellerIdAsync(sellerId);
            return Ok(orders);
        }

        /// <summary>
        /// Creates a new order in the system.
        /// </summary>
        /// <param name="order">The order information to create.</param>
        /// <returns>The newly created order with its generated ID.</returns>
        /// <response code="201">Returns the newly created order</response>
        /// <response code="400">If the order data is invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointName("CreateOrder")]
        [EndpointSummary("Creates a new order in the system.")]
        [EndpointDescription("Creates a new order in the system. The order information must be provided in the request body.")]
        
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            var createdOrder = await orderService.CreateOrderAsync(order);
            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
        }

        /// <summary>
        /// Updates the status of an existing order.
        /// </summary>
        /// <param name="id">The unique identifier of the order to update.</param>
        /// <param name="status">The new status value (e.g., "pending", "confirmed", "shipped", "delivered", "cancelled").</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">If the status was successfully updated</response>
        /// <response code="404">If the order with the specified ID doesn't exist</response>
        [HttpPut("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointName("UpdateOrderStatus")]
        [EndpointSummary("Updates the status of an order.")]
        [EndpointDescription("Updates the status of an existing order. Valid status values include: pending, confirmed, shipped, delivered, cancelled.")]
        
        public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] string status)
        {
            var order = await orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await orderService.UpdateOrderStatusAsync(id, status);
            return NoContent();
        }

        /// <summary>
        /// Updates the shipping information for an existing order.
        /// </summary>
        /// <param name="id">The unique identifier of the order to update.</param>
        /// <param name="shippingInfo">The new shipping information.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">If the shipping information was successfully updated</response>
        /// <response code="404">If the order with the specified ID doesn't exist</response>
        [HttpPut("{id}/shipping")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointName("UpdateShippingInfo")]
        [EndpointSummary("Updates the shipping details for an order.")]
        [EndpointDescription("Updates the shipping information for an existing order including address, tracking number, and delivery estimates.")]
        
        public async Task<IActionResult> UpdateShippingInfo(Guid id, [FromBody] ShippingInfo shippingInfo)
        {
            var order = await orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await orderService.UpdateShippingInfoAsync(id, shippingInfo);
            return NoContent();
        }

        /// <summary>
        /// Deletes an order from the system.
        /// </summary>
        /// <param name="id">The unique identifier of the order to delete.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">If the order was successfully deleted</response>
        /// <response code="404">If the order with the specified ID doesn't exist</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointName("DeleteOrder")]
        [EndpointSummary("Removes an order from the system.")]
        [EndpointDescription("Permanently deletes an order from the database. This operation cannot be undone.")]
        
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await orderService.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}
