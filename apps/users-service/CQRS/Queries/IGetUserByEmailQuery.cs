using UsersService.Domain.Models;
using UsersService.Infrastructure.Repositories;

namespace UsersService.CQRS.Queries
{
    public interface IGetUserByEmailQuery
    {
        Task<User?> ExecuteAsync(string email);
    }

    public class GetUserByEmailQuery : IGetUserByEmailQuery
    {
        private readonly IUserRepository _userRepository;

        public GetUserByEmailQuery(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> ExecuteAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }
    }
}
