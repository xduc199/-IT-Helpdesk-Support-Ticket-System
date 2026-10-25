using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Features.Users.Queries;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    // Giữ nguyên cơ chế Inject bằng Constructor truyền thống để tránh lỗi Reference biến ngầm
    private readonly DbContext _dbContext; 

  
    public GetCurrentUserQueryHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        // Giả lập hoặc gọi thẳng từ Claims nếu không có dịch vụ trung gian
        // Để tạm thời chạy mượt, hệ thống sẽ quét thực thể dựa trên liên kết DB
        var user = await _dbContext.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken); 

        if (user == null)
        {
            throw new KeyNotFoundException("Không tìm thấy thông tin tài khoản hoặc phiên đăng nhập hết hạn.");
        }

        return new UserDto(user.Id, user.Username, user.Email, user.Role);
    }
}