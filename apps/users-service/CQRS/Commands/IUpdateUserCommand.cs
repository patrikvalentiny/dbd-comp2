using UsersService.Domain.Models;
using UsersService.Infrastructure.Repositories;

namespace UsersService.CQRS.Commands
{
    public interface IUpdateUserCommand
    {
        Task ExecuteAsync(User user);
    }

    public class UpdateUserCommand : IUpdateUserCommand
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionManager _transactionManager;

        public UpdateUserCommand(IUserRepository userRepository, ITransactionManager transactionManager)
        {
            _userRepository = userRepository;
            _transactionManager = transactionManager;
        }

        public async Task ExecuteAsync(User user)
        {
            try
            {
                await _transactionManager.BeginTransactionAsync();
                await _userRepository.UpdateUserAsync(user);
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
