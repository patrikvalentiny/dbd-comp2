using UsersService.Domain.Models;
using UsersService.Infrastructure.Repositories;

namespace UsersService.CQRS.Queries
{
    public interface IGetAllUsersQuery
    {
        Task<IEnumerable<User>> ExecuteAsync();
    }

    public class GetAllUsersQuery : IGetAllUsersQuery
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQuery(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> ExecuteAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
    }
}
