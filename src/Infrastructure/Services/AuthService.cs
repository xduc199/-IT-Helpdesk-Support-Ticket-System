using System;
using System.Threading.Tasks;
using Core.DTOs;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class AuthService(ApplicationDbContext context, TokenService tokenService)
    {
        public async Task<User?> RegisterAsync(RegisterDto dto)
        {
            if (await context.Users.AnyAsync(x => x.Username == dto.Username))
                return null;

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "User",
                RefreshToken = tokenService.GenerateRefreshToken(),
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> LoginAsync(LoginDto dto)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            user.RefreshToken = tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await context.SaveChangesAsync();

            return user;
        }

      public async Task<User?> RefreshTokenAsync(RefreshTokenRequestDto dto)
{
    var principal = tokenService.GetPrincipalFromExpiredToken(dto.Token);
    if (principal == null) return null;

    var username = principal.Identity?.Name;
    if (username == null) return null;

    var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
    if (user == null) return null;

    if (user.RefreshToken != dto.RefreshToken)
        return null;

    if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        return null;

    // rotate token (quan trọng)
    user.RefreshToken = tokenService.GenerateRefreshToken();
    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

    await context.SaveChangesAsync();
    return user;
}
        public async Task<bool> AssignRoleAsync(AssignRoleDto dto)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null) return false;

            user.Role = dto.Role;
            var rowsAffected = await context.SaveChangesAsync();
            return rowsAffected > 0;
        }
    }
}