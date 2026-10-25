using Core.Entities;

namespace Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email); // Đã thêm
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task SaveChangesAsync(); // Đã thêm
}