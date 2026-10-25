using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Services;

public class TicketHideTests
{
    [Fact]
    public async Task HideTicket_ShouldSet_IsHidden_True()
    {
        var ticket = new Ticket { Id = Guid.NewGuid(), Title = "Test" };

        var repo = new Mock<ITicketRepository>();
        repo.Setup(x => x.GetByIdAsync(ticket.Id)).ReturnsAsync(ticket);
        repo.Setup(x => x.UpdateAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);

        var service = new TicketService(repo.Object, null!);

        var result = await service.HideTicketAsync(ticket.Id, Guid.NewGuid());

        Assert.True(result);
        Assert.True(ticket.IsHidden);
    }

    [Fact]
    public async Task UnhideTicket_ShouldSet_IsHidden_False()
    {
        var ticket = new Ticket { Id = Guid.NewGuid(), IsHidden = true };

        var repo = new Mock<ITicketRepository>();
        repo.Setup(x => x.GetByIdAsync(ticket.Id)).ReturnsAsync(ticket);
        repo.Setup(x => x.UpdateAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);

        var service = new TicketService(repo.Object, null!);

        var result = await service.UnhideTicketAsync(ticket.Id, Guid.NewGuid());

        Assert.True(result);
        Assert.False(ticket.IsHidden);
    }
}