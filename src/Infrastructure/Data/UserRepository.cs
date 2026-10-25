using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id) 
        => await context.Users.FindAsync(id);

    public async Task<User?> GetByUsernameAsync(string username)
        => await context.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User?> GetByEmailAsync(string email)
        => await context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task AddAsync(User user) 
        => await context.Users.AddAsync(user);

    public async Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync() 
        => await context.SaveChangesAsync();
}