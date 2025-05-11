using UsersService.Domain.Models;
using UsersService.Infrastructure.Repositories;

namespace UsersService.CQRS.Queries
{
    public interface IGetUserByIdQuery
    {
        Task<User?> ExecuteAsync(Guid userId);
    }

    public class GetUserByIdQuery : IGetUserByIdQuery
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQuery(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> ExecuteAsync(Guid userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }
    }
}
