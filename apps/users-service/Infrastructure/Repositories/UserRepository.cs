using Microsoft.EntityFrameworkCore;
using UsersService.Domain.Models;
using UsersService.Infrastructure.Data;

namespace UsersService.Infrastructure.Repositories
{
    public class UserRepository(UserDbContext context) : IUserRepository
    {
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await context.Users.FindAsync(user.Id);
            if (existingUser != null)
            {
                context.Entry(existingUser).CurrentValues.SetValues(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await context.Users.FindAsync(id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }
    }
}