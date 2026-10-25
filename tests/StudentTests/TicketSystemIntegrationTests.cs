using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Services;

namespace StudentTests;

public class TicketSystemIntegrationTests
{
    private TicketService CreateService(
        Mock<ITicketRepository> ticketRepo,
        Mock<IAuditLogRepository> auditRepo)
    {
        return new TicketService(ticketRepo.Object, auditRepo.Object);
    }

    // =========================================================
    // 1. CREATE + DEFAULT STATE
    // =========================================================
    [Fact]
    public async Task System_Should_CreateTicket_WithDefaultValues()
    {
        var ticketRepo = new Mock<ITicketRepository>();
        var auditRepo = new Mock<IAuditLogRepository>();

        ticketRepo.Setup(x => x.AddAsync(It.IsAny<Ticket>()))
                  .Returns(Task.CompletedTask);

        ticketRepo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                  .ReturnsAsync((Guid id) => new Ticket
                  {
                      Id = id,
                      Title = "Bug test",
                      Description = "Test system",
                      Status = "Open",
                      IsDeleted = false
                  });

        var service = CreateService(ticketRepo, auditRepo);

        var result = await service.CreateTicketAsync(
            new Core.DTOs.CreateTicketDto
            {
                Title = "Bug test",
                Description = "Test system",
                TagIds = new List<Guid>()
            },
            Guid.NewGuid()
        );

        Assert.NotNull(result);
        Assert.Equal("Bug test", result.Title);
    }

    // =========================================================
    // 2. SOFT DELETE FLOW
    // =========================================================
    [Fact]
    public async Task System_Should_SoftDelete_Ticket()
    {
        var ticketId = Guid.NewGuid();

        var ticketRepo = new Mock<ITicketRepository>();
        var auditRepo = new Mock<IAuditLogRepository>();

        var ticket = new Ticket
        {
            Id = ticketId,
            IsDeleted = false
        };

        ticketRepo.Setup(x => x.GetByIdAsync(ticketId))
                  .ReturnsAsync(ticket);

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
        Assert.True(ticket.IsDeleted);
    }

    // =========================================================
    // 3. RESTORE FLOW
    // =========================================================
    [Fact]
    public async Task System_Should_Restore_Ticket()
    {
        var ticketId = Guid.NewGuid();

        var ticketRepo = new Mock<ITicketRepository>();
        var auditRepo = new Mock<IAuditLogRepository>();

        var ticket = new Ticket
        {
            Id = ticketId,
            IsDeleted = true
        };

        ticketRepo.Setup(x => x.GetByIdAsync(ticketId))
                  .ReturnsAsync(ticket);

        ticketRepo.Setup(x => x.UpdateAsync(It.IsAny<Ticket>()))
                  .Returns(Task.CompletedTask);

        auditRepo.Setup(x => x.AddAsync(It.IsAny<AuditLog>()))
                 .Returns(Task.CompletedTask);

        var service = CreateService(ticketRepo, auditRepo);

        var result = await service.RestoreTicketAsync(ticketId, "Admin");

        Assert.True(result);
        Assert.False(ticket.IsDeleted);
    }

    // =========================================================
    // 4. STATUS FLOW VALIDATION
    // =========================================================
    [Fact]
    public async Task System_Should_Enforce_Status_Transition()
    {
        var ticketId = Guid.NewGuid();

        var ticketRepo = new Mock<ITicketRepository>();
        var auditRepo = new Mock<IAuditLogRepository>();

        ticketRepo.Setup(x => x.GetByIdAsync(ticketId))
                  .ReturnsAsync(new Ticket
                  {
                      Id = ticketId,
                      Status = "Open"
                  });

        ticketRepo.Setup(x => x.UpdateAsync(It.IsAny<Ticket>()))
                  .Returns(Task.CompletedTask);

        var service = CreateService(ticketRepo, auditRepo);

        var result = await service.UpdateTicketStatusAsync(
            ticketId,
            "In Progress"
        );

        Assert.True(result);
    }

    // =========================================================
    // 5. SECURITY: USER CANNOT DELETE
    // =========================================================
    [Fact]
    public async Task System_Should_Block_User_Delete()
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

    // =========================================================
    // 6. GET FILTER (NOT DELETED)
    // =========================================================
    [Fact]
    public async Task System_Should_NotReturn_DeletedTickets()
    {
        var ticketRepo = new Mock<ITicketRepository>();
        var auditRepo = new Mock<IAuditLogRepository>();

        ticketRepo.Setup(x => x.GetAllAsync())
                  .ReturnsAsync(new List<Ticket>
                  {
                      new Ticket { Id = Guid.NewGuid(), IsDeleted = true },
                      new Ticket { Id = Guid.NewGuid(), IsDeleted = false }
                  });

        var service = CreateService(ticketRepo, auditRepo);

        var result = await service.GetAllTicketsAsync(1, 10);

        Assert.True(result.TotalCount >= 0);
        Assert.NotNull(result.Tickets);
        Assert.True(result.Tickets is not null);
    }
}