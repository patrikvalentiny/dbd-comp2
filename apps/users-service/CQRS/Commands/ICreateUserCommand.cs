using UsersService.Domain.Models;
using UsersService.Infrastructure.Repositories;

namespace UsersService.CQRS.Commands
{
    public interface ICreateUserCommand
    {
        Task<User> ExecuteAsync(User user);
    }

    public class CreateUserCommand : ICreateUserCommand
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionManager _transactionManager;

        public CreateUserCommand(IUserRepository userRepository, ITransactionManager transactionManager)
        {
            _userRepository = userRepository;
            _transactionManager = transactionManager;
        }

        public async Task<User> ExecuteAsync(User user)
        {
            try
            {
                await _transactionManager.BeginTransactionAsync();
                var createdUser = await _userRepository.CreateUserAsync(user);
                await _transactionManager.CommitTransactionAsync();
                return createdUser;
            }
            catch
            {
                await _transactionManager.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
