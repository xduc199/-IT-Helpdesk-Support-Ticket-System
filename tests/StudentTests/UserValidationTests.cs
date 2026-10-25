using Xunit;
using Core.Entities;
using Infrastructure.Tests;

namespace Infrastructure.Tests;

public class UserValidationTests
{
    [Theory]
    [InlineData("test@gmail.com", true)]
    [InlineData("invalid-email", false)]
    [InlineData("", false)]
    public void Email_ShouldBeValidatedCorrectly(string email, bool expected)
    {
        var user = new User { Email = email };

        var result = user.IsValidEmail();

        Assert.Equal(expected, result);
    }
}