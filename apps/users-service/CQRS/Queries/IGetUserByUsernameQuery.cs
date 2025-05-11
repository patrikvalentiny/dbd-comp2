using UsersService.Domain.Models;
using UsersService.Infrastructure.Repositories;

namespace UsersService.CQRS.Queries
{
    public interface IGetUserByUsernameQuery
    {
        Task<User?> ExecuteAsync(string username);
    }

    public class GetUserByUsernameQuery : IGetUserByUsernameQuery
    {
        private readonly IUserRepository _userRepository;

        public GetUserByUsernameQuery(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> ExecuteAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }
    }
}
