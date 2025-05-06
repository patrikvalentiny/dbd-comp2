using UsersService.Domain.Models;
using UsersService.Infrastructure.Repositories;

namespace UsersService.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await userRepository.GetAllUsersAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await userRepository.GetUserByIdAsync(id);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await userRepository.GetUserByUsernameAsync(username);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await userRepository.GetUserByEmailAsync(email);
    }

    public async Task<User> CreateUserAsync(User user)
    {

        return await userRepository.CreateUserAsync(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        await userRepository.UpdateUserAsync(user);
    }

    public async Task DeleteUserAsync(Guid id)
    {
        await userRepository.DeleteUserAsync(id);
    }
}

