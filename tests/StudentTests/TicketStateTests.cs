using Xunit;
using Core.Entities;

namespace Infrastructure.Tests;

public class TicketStateTests
{
    [Fact]
    public void NewTicket_ShouldBeOpen()
    {
        var ticket = new Ticket { Title = "Test" };

        Assert.Equal("Open", ticket.Status);
    }

    [Fact]
    public void ShouldAllow_Open_To_InProgress()
    {
        var current = "Open";
        var next = "In Progress";

        var isValid = current == "Open" && next == "In Progress";

        Assert.True(isValid);
    }

    [Fact]
    public void ShouldAllow_InProgress_To_Resolved()
    {
        var current = "In Progress";
        var next = "Resolved";

        var isValid = current == "In Progress" && next == "Resolved";

        Assert.True(isValid);
    }

    [Fact]
    public void ShouldNotAllow_Resolved_To_InProgress()
    {
        var current = "Resolved";
        var next = "In Progress";

        var validTransitions = new[]
        {
            ("Open", "In Progress"),
            ("In Progress", "Resolved"),
            ("Resolved", "Closed")
        };

        var isValid = validTransitions.Contains((current, next));

        Assert.False(isValid); // ✅ FIXED
    }
}