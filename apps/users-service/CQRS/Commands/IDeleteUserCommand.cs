using MessageClient.Interfaces;
using Messages;
using UsersService.Infrastructure.Repositories;

namespace UsersService.CQRS.Commands
{
    public interface IDeleteUserCommand
    {
        Task ExecuteAsync(Guid userId);
    }

    public class DeleteUserCommand : IDeleteUserCommand
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionManager _transactionManager;
        private readonly IMessagingClient _messagingClient;
        private readonly ILogger<DeleteUserCommand> _logger;

        public DeleteUserCommand(
            IUserRepository userRepository,
            ITransactionManager transactionManager,
            IMessagingClient messagingClient,
            ILogger<DeleteUserCommand> logger)
        {
            _userRepository = userRepository;
            _transactionManager = transactionManager;
            _messagingClient = messagingClient;
            _logger = logger;
        }

        public async Task ExecuteAsync(Guid id)
        {
            await _transactionManager.BeginTransactionAsync();

            try
            {
                await _userRepository.DeleteUserAsync(id);

                // Publish the user deleted event
                var userDeletedEvent = new UserDeleted
                {
                    UserId = id
                };

                await _messagingClient.PublishAsync("user_deleted", userDeletedEvent);
                _logger.LogInformation("Published UserDeleted event for user {UserId}", id);

                await _transactionManager.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _transactionManager.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to delete user {UserId}", id);
                throw;
            }
        }
    }
}
