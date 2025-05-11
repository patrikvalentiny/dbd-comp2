using OrdersService.Infrastructure.Repositories;

namespace OrdersService.CQRS.Commands
{
    public class DeleteOrderCommand : IDeleteOrderCommand
    {
        private readonly IOrderRepository _orderRepository;

        public DeleteOrderCommand(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task ExecuteAsync(Guid id)
        {
            await _orderRepository.DeleteOrderAsync(id);
        }
    }
}
