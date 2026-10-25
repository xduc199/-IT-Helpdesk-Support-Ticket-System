using Core.DTOs;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserMeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserMeDto>> GetMe()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim) ||
            !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Token không hợp lệ hoặc không có quyền truy cập!");
        }

        var user = await context.Users.FindAsync(userId);

        if (user == null)
            return NotFound("Không tìm thấy người dùng!");

        var userMe = new UserMeDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };

        return Ok(userMe);
    }
}