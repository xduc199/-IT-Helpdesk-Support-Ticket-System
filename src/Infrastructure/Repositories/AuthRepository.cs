using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public interface IAuthRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddUserAsync(User user);
}

public class AuthRepository(ApplicationDbContext context) : IAuthRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddUserAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }
}