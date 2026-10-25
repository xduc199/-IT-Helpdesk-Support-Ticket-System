using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Services;

namespace StudentTests;

public class TicketDeleteRestoreTests
{
    private TicketService CreateService(
        Mock<ITicketRepository> ticketRepo,
        Mock<IAuditLogRepository> auditRepo)
    {
        return new TicketService(
            ticketRepo.Object,
            auditRepo.Object
        );
    }

    // =========================
    // DELETE SUCCESS (ADMIN)
    // =========================
    [Fact]
    public async Task DeleteTicket_ShouldReturnTrue_WhenAdmin()
    {
        var ticketId = Guid.NewGuid();

        var ticketRepo = new Mock<ITicketRepository>();
        var auditRepo = new Mock<IAuditLogRepository>();

        ticketRepo.Setup(x => x.GetByIdAsync(ticketId))
                  .ReturnsAsync(new Ticket { Id = ticketId });

        ticketRepo.Setup(x => x.UpdateAsync(It.IsAny<Ticket>()))
                  .Returns(Task.CompletedTask);

        auditRepo.Setup(x => x.AddAsync(It.IsAny<AuditLog>()))
                 .Returns(Task.CompletedTask);

        var service = CreateService(ticketRepo, auditRepo);

        var result = await service.DeleteTicketAsync(
            ticketId,
            "Admin",
            Guid.NewGuid()
        );

        Assert.True(result);
    }

    // =========================
    // DELETE FAIL (USER)
    // =========================
    [Fact]
    public async Task DeleteTicket_ShouldFail_WhenUser()
    {
        var ticketId = Guid.NewGuid();

        var ticketRepo = new Mock<ITicketRepository>();
        var auditRepo = new Mock<IAuditLogRepository>();

        ticketRepo.Setup(x => x.GetByIdAsync(ticketId))
                  .ReturnsAsync(new Ticket { Id = ticketId });

        var service = CreateService(ticketRepo, auditRepo);

        var result = await service.DeleteTicketAsync(
            ticketId,
            "User",
            Guid.NewGuid()
        );

        Assert.False(result);
    }

    // =========================
    // RESTORE SUCCESS
    // =========================
    [Fact]
    public async Task RestoreTicket_ShouldReturnTrue_WhenAdmin()
    {
        var ticketId = Guid.NewGuid();

        var ticketRepo = new Mock<ITicketRepository>();
        var auditRepo = new Mock<IAuditLogRepository>();

        ticketRepo.Setup(x => x.GetByIdAsync(ticketId))
                  .ReturnsAsync(new Ticket
                  {
                      Id = ticketId,
                      IsDeleted = true
                  });

        ticketRepo.Setup(x => x.UpdateAsync(It.IsAny<Ticket>()))
                  .Returns(Task.CompletedTask);

        auditRepo.Setup(x => x.AddAsync(It.IsAny<AuditLog>()))
                 .Returns(Task.CompletedTask);

        var service = CreateService(ticketRepo, auditRepo);

        var result = await service.RestoreTicketAsync(
            ticketId,
            "Admin"
        );

        Assert.True(result);
    }

    // =========================
    // RESTORE FAIL
    // =========================
    [Fact]
    public async Task RestoreTicket_ShouldFail_WhenUser()
    {
        var ticketId = Guid.NewGuid();

        var ticketRepo = new Mock<ITicketRepository>();
        var auditRepo = new Mock<IAuditLogRepository>();

        ticketRepo.Setup(x => x.GetByIdAsync(ticketId))
                  .ReturnsAsync(new Ticket
                  {
                      Id = ticketId,
                      IsDeleted = true
                  });

        var service = CreateService(ticketRepo, auditRepo);

        var result = await service.RestoreTicketAsync(
            ticketId,
            "User"
        );

        Assert.False(result);
    }
}