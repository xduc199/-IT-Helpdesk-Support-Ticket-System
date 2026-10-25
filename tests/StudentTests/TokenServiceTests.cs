using Xunit;
using Infrastructure.Services;
using Core.Entities;
using Microsoft.Extensions.Configuration;
using Moq;
using System;

namespace Infrastructure.Tests;

public class TokenServiceTests
{
    [Fact]
    public void CreateToken_ShouldReturnValidString()
    {
        var mockConfig = new Mock<IConfiguration>();

        // ✅ FIX KEY >= 32 bytes (quan trọng)
        mockConfig.Setup(c => c["Jwt:Key"])
            .Returns("12345678901234567890123456789012");

        mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("Nhom14");
        mockConfig.Setup(c => c["Jwt:Audience"]).Returns("Users");

        var service = new TokenService(mockConfig.Object);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "admin",
            Role = "Admin"
        };

        var token = service.CreateToken(user);

        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public void Tokens_ShouldBeDifferent_ForDifferentUsers()
    {
        var mockConfig = new Mock<IConfiguration>();

        // ✅ FIX KEY
        mockConfig.Setup(c => c["Jwt:Key"])
            .Returns("12345678901234567890123456789012");

        mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("Nhom14");
        mockConfig.Setup(c => c["Jwt:Audience"]).Returns("Users");

        var service = new TokenService(mockConfig.Object);

        var user1 = new User { Id = Guid.NewGuid(), Username = "u1", Role = "User" };
        var user2 = new User { Id = Guid.NewGuid(), Username = "u2", Role = "User" };

        var token1 = service.CreateToken(user1);
        var token2 = service.CreateToken(user2);

        Assert.NotEqual(token1, token2);
    }
}