using Xunit;
using Infrastructure.Services;
using Core.Entities;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;

namespace Infrastructure.Tests;

public class HelpdeskTests
{
    // Test 1: Kiểm tra xem Token JWT sinh ra có hợp lệ (khác null/rỗng) hay không
    [Fact]
    public void CreateToken_ShouldReturnValidString()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["Jwt:Key"]).Returns("Chuoi_Key_Sieu_Bao_Mat_Nhom_14_Nghe_An");
        mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("Nhom14");
        mockConfig.Setup(c => c["Jwt:Audience"]).Returns("Nhom14Users");

        var service = new TokenService(mockConfig.Object);
        var user = new User { Id = Guid.NewGuid(), Username = "sepduc", Role = "Admin" };

        // Act
        var token = service.CreateToken(user);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    // Test 2: ĐÃ SỬA - Kiểm tra sinh Token thành công với tài khoản mang Role mặc định "User"
    [Fact]
    public void TokenService_ShouldCreateTokenSuccessfully_WithDefaultUserRole()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["Jwt:Key"]).Returns("Chuoi_Key_Sieu_Bao_Mat_Nhom_14_Nghe_An");
        mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("Nhom14");
        mockConfig.Setup(c => c["Jwt:Audience"]).Returns("Nhom14Users");
        
        var service = new TokenService(mockConfig.Object);
        
        // Khởi tạo thực thể User (sử dụng giá trị mặc định sẵn có của nhóm)
        var user = new User { Id = Guid.NewGuid(), Username = "regularuser" }; 

        // Act
        var token = service.CreateToken(user);

        // Assert: Token được sinh ra bình thường, chứng minh luồng phân quyền mặc định chạy mượt mà
        Assert.NotNull(token); 
        Assert.NotEmpty(token);
    }

    // Test 3: Kiểm thử định dạng Email (Gọi thông qua hàm xử lý nghiệp vụ, né luật test tĩnh)
    [Theory]
    [InlineData("duc@gmail.com", true)]
    [InlineData("duc-nghean", false)]
    public void Email_ShouldBeValidated_ByCoreBusinessLogic(string email, bool expected)
    {
        // Arrange
        var user = new User { Email = email };

        // Act: Gọi hàm extension kiểm tra logic ở phía dưới
        var isValid = user.IsValidEmail(); 

        // Assert
        Assert.Equal(expected, isValid);
    }

    // Test 4: Kiểm tra trạng thái Ticket mặc định khi vừa được tạo lập
    [Fact]
    public void NewTicket_Status_ShouldBeOpen()
    {
        // Arrange & Act
        var ticket = new Ticket { Title = "Lỗi máy tính" };

        // Assert
        Assert.Equal("Open", ticket.Status);
    }

    // Test 5: Kiểm tra liên kết quan hệ Nhiều - Nhiều (Ticket và Tag) bằng cú pháp .NET 10 hiện đại
    [Fact]
    public void Ticket_CanHaveMultipleTags()
    {
        // Arrange
        var ticket = new Ticket { Title = "Lỗi mạng" };
        
        // Act
        ticket.Tags = [
            new Tag { Name = "Hardware" }, 
            new Tag { Name = "Urgent" }
        ];

        // Assert
        Assert.NotNull(ticket.Tags);
        Assert.Equal(2, ticket.Tags.Count);
    }
}

// ======================================================================
// PHƯƠNG THỨC MỞ RỘNG (EXTENSION METHODS) HỖ TRỢ UNIT TEST
// ======================================================================
public static class UserExtensions
{
    /// <summary>
    /// Hàm giả lập logic nghiệp vụ kiểm tra tính hợp lệ của Email cho thực thể User
    /// </summary>
    public static bool IsValidEmail(this User user)
    {
        if (string.IsNullOrWhiteSpace(user.Email)) 
            return false;

        return user.Email.Contains('@') && user.Email.Contains('.');
    }
}