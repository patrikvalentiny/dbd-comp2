using UsersService.CQRS.Commands;
using UsersService.CQRS.Queries;
using UsersService.Domain.Models;

namespace UsersService.Services;

public class UserService : IUserService
{
    private readonly IGetAllUsersQuery _getAllUsersQuery;
    private readonly IGetUserByIdQuery _getUserByIdQuery;
    private readonly IGetUserByUsernameQuery _getUserByUsernameQuery;
    private readonly IGetUserByEmailQuery _getUserByEmailQuery;
    private readonly ICreateUserCommand _createUserCommand;
    private readonly IUpdateUserCommand _updateUserCommand;
    private readonly IDeleteUserCommand _deleteUserCommand;

    public UserService(
        IGetAllUsersQuery getAllUsersQuery,
        IGetUserByIdQuery getUserByIdQuery,
        IGetUserByUsernameQuery getUserByUsernameQuery,
        IGetUserByEmailQuery getUserByEmailQuery,
        ICreateUserCommand createUserCommand,
        IUpdateUserCommand updateUserCommand,
        IDeleteUserCommand deleteUserCommand)
    {
        _getAllUsersQuery = getAllUsersQuery;
        _getUserByIdQuery = getUserByIdQuery;
        _getUserByUsernameQuery = getUserByUsernameQuery;
        _getUserByEmailQuery = getUserByEmailQuery;
        _createUserCommand = createUserCommand;
        _updateUserCommand = updateUserCommand;
        _deleteUserCommand = deleteUserCommand;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _getAllUsersQuery.ExecuteAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _getUserByIdQuery.ExecuteAsync(id);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _getUserByUsernameQuery.ExecuteAsync(username);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _getUserByEmailQuery.ExecuteAsync(email);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        return await _createUserCommand.ExecuteAsync(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        await _updateUserCommand.ExecuteAsync(user);
    }

    public async Task DeleteUserAsync(Guid id)
    {
        await _deleteUserCommand.ExecuteAsync(id);
    }
}

