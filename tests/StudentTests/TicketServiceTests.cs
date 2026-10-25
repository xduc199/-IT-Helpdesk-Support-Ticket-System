using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Services;

namespace StudentTests;

public class TicketServiceTests
{
    private TicketService CreateService(Mock<ITicketRepository> ticketRepo)
    {
        var auditRepo = new Mock<IAuditLogRepository>();

        return new TicketService(
            ticketRepo.Object,
            auditRepo.Object
        );
    }

    // =========================
    // TEST 1: CREATE TICKET
    // =========================
    [Fact]
    public async Task CreateTicket_ShouldReturnTicket()
    {
        var ticketRepo = new Mock<ITicketRepository>();

        ticketRepo.Setup(x => x.AddAsync(It.IsAny<Ticket>()))
                  .Returns(Task.CompletedTask);

        ticketRepo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                  .ReturnsAsync(new Ticket
                  {
                      Id = Guid.NewGuid(),
                      Title = "Test",
                      Status = "Open"
                  });

        var service = CreateService(ticketRepo);

        var result = await service.CreateTicketAsync(
            new Core.DTOs.CreateTicketDto
            {
                Title = "Test",
                Description = "Desc",
                TagIds = new List<Guid>()
            },
            Guid.NewGuid()
        );

        Assert.NotNull(result);
        Assert.Equal("Test", result.Title);
    }

    // =========================
    // TEST 2: ASSIGN TICKET
    // =========================
    [Fact]
    public async Task AssignTicket_ShouldReturnTrue()
    {
        var ticketId = Guid.NewGuid();

        var ticketRepo = new Mock<ITicketRepository>();

        ticketRepo.Setup(x => x.GetByIdAsync(ticketId))
                  .ReturnsAsync(new Ticket { Id = ticketId });

        ticketRepo.Setup(x => x.UpdateAsync(It.IsAny<Ticket>()))
                  .Returns(Task.CompletedTask);

        var service = CreateService(ticketRepo);

        var result = await service.AssignTicketAsync(ticketId, Guid.NewGuid());

        Assert.True(result);
    }

    // =========================
    // TEST 3: STATUS UPDATE
    // =========================
    [Fact]
    public async Task UpdateStatus_ShouldWork()
    {
        var ticketId = Guid.NewGuid();

        var ticketRepo = new Mock<ITicketRepository>();

        ticketRepo.Setup(x => x.GetByIdAsync(ticketId))
                  .ReturnsAsync(new Ticket { Id = ticketId, Status = "Open" });

        ticketRepo.Setup(x => x.UpdateAsync(It.IsAny<Ticket>()))
                  .Returns(Task.CompletedTask);

        var service = CreateService(ticketRepo);

        var result = await service.UpdateTicketStatusAsync(ticketId, "In Progress");

        Assert.True(result);
    }
}