using System.Text.Json.Serialization;
using Core.DTOs;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    AuthService authService,
    TokenService tokenService) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> Register(
        [FromBody] RegisterDto dto)
    {
        var user = await authService.RegisterAsync(dto);

        if (user == null)
            return BadRequest("Tài khoản này đã đăng ký rồi!");

        return Ok(new AuthResponseDto
        {
            Token = tokenService.CreateToken(user),
            RefreshToken = user.RefreshToken ?? string.Empty,
            Username = user.Username,
            Role = user.Role
        });
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> Login(
        [FromBody] LoginDto dto)
    {
        var user = await authService.LoginAsync(dto);

        if (user == null)
            return Unauthorized("Sai tài khoản hoặc mật khẩu rồi!");

        return Ok(new AuthResponseDto
        {
            Token = tokenService.CreateToken(user),
            RefreshToken = user.RefreshToken ?? string.Empty,
            Username = user.Username,
            Role = user.Role
        });
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> Refresh(
        [FromBody] RefreshTokenRequestDto dto)
    {
        var user = await authService.RefreshTokenAsync(dto);

        if (user == null)
            return BadRequest("Refresh Token không hợp lệ hoặc đã hết hạn!");

        return Ok(new AuthResponseDto
        {
            Token = tokenService.CreateToken(user),
            RefreshToken = user.RefreshToken ?? string.Empty,
            Username = user.Username,
            Role = user.Role
        });
    }

    [HttpPost("assign-role")]
    [ProducesResponseType(typeof(ApiMessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiMessageDto>> AssignRole(
        [FromBody] AssignRoleDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username) ||
            string.IsNullOrWhiteSpace(dto.Role))
        {
            return BadRequest("Dữ liệu không hợp lệ.");
        }

        var result = await authService.AssignRoleAsync(dto);

        if (!result)
            return BadRequest("Cập nhật quyền thất bại hoặc không tìm thấy tài khoản.");

        return Ok(new ApiMessageDto
        {
            Message = $"Đã cấp quyền '{dto.Role}' cho tài khoản '{dto.Username}' thành công!"
        });
    }
}