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

        public DeleteUserCommand(IUserRepository userRepository, ITransactionManager transactionManager)
        {
            _userRepository = userRepository;
            _transactionManager = transactionManager;
        }

        public async Task ExecuteAsync(Guid userId)
        {
            try
            {
                await _transactionManager.BeginTransactionAsync();
                await _userRepository.DeleteUserAsync(userId);
                await _transactionManager.CommitTransactionAsync();
            }
            catch
            {
                await _transactionManager.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
