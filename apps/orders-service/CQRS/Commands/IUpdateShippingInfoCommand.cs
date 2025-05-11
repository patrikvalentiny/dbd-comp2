using OrdersService.Domain.Models;

namespace OrdersService.CQRS.Commands
{
    public interface IUpdateShippingInfoCommand
    {
        Task ExecuteAsync(Guid orderId, ShippingInfo shippingInfo);
    }
}
