using MediatR;
using System;

namespace Core.Features.Auth.Commands;

public record AuthResponseDto(string AccessToken, string RefreshToken, DateTime ExpiresAt);

public record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<AuthResponseDto>;