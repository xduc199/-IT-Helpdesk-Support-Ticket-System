using MediatR;
using System;

namespace Core.Features.Users.Queries;

// Dto sạch trả về cho Client, không kèm PasswordHash
public record UserDto(Guid Id, string Username, string Email, string Role);

// Request Query sử dụng Primary Constructor C# 14
public record GetCurrentUserQuery : IRequest<UserDto>;