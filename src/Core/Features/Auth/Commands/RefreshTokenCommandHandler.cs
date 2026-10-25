using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Features.Auth.Commands;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly DbContext _dbContext;

    public RefreshTokenCommandHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // Tìm User dựa trên Refresh Token cũ gửi lên
        var user = await _dbContext.Set<User>()
            .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken, cancellationToken);

        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new ArgumentException("Refresh Token không hợp lệ hoặc đã hết hạn. Vui lòng đăng nhập lại.");
        }

        // Logic sinh chuỗi chuỗi token ngẫu nhiên mới an toàn
        var newAccessToken = Guid.NewGuid().ToString() + "-jwt-mock-token"; 
        var newRefreshToken = Guid.NewGuid().ToString();

        // Cập nhật lại vào Database
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto(newAccessToken, newRefreshToken, DateTime.UtcNow.AddMinutes(15));
    }
}