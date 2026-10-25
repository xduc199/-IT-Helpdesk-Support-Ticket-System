using Core.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services;

public class TicketService(
    ITicketRepository ticketRepo,
    IAuditLogRepository auditRepo
) : ITicketService
{
    // =========================
    // GET ALL
    // =========================
    public async Task<(IEnumerable<TicketDto> Tickets, int TotalCount)> GetAllTicketsAsync(int pageNumber, int pageSize)
    {
        var allTickets = await ticketRepo.GetAllAsync();

        var ticketsList = allTickets
            .Where(t => !t.IsDeleted)
            .ToList();

        var totalCount = ticketsList.Count;

        var pagedTickets = ticketsList
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var ticketDtos = pagedTickets.Select(t => new TicketDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status,
            RequesterName = t.Requester?.Username ?? "Ẩn danh",
            Tags = t.Tags?.Select(tag => tag.Name).ToList() ?? new List<string>()
        });

        return (ticketDtos, totalCount);
    }

    // =========================
    // GET BY ID
    // =========================
    public async Task<TicketDto?> GetTicketByIdAsync(Guid id)
    {
        var t = await ticketRepo.GetByIdAsync(id);

        if (t == null || t.IsDeleted) return null;

        return new TicketDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status,
            RequesterName = t.Requester?.Username ?? "Ẩn danh",
            Tags = t.Tags?.Select(tag => tag.Name).ToList() ?? new List<string>()
        };
    }

    // =========================
    // CREATE
    // =========================
    public async Task<TicketDto> CreateTicketAsync(CreateTicketDto dto, Guid requesterId)
    {
        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Status = "Open",
            CreatedAt = DateTime.UtcNow,
            RequesterId = requesterId,
            Tags = dto.TagIds.Select(tagId => new Tag { Id = tagId }).ToList(),
            IsDeleted = false
        };

        await ticketRepo.AddAsync(ticket);

        return new TicketDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status,
            RequesterName = "Hệ thống vừa tạo",
            Tags = ticket.Tags?.Select(t => t.Name).ToList() ?? new List<string>()
        };
    }

    // =========================
    // DELETE (SOFT DELETE + AUDIT)
    // =========================
    public async Task<bool> DeleteTicketAsync(Guid ticketId, string role, Guid userId)
    {
        var ticket = await ticketRepo.GetByIdAsync(ticketId);

        if (ticket == null || ticket.IsDeleted)
            return false;

        if (role == "User")
            return false;

        ticket.IsDeleted = true;
        ticket.DeletedAt = DateTime.UtcNow;
        ticket.DeletedBy = userId;

        await ticketRepo.UpdateAsync(ticket);

        await auditRepo.AddAsync(new AuditLog
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            UserId = userId,
            Action = "DELETE_TICKET",
            CreatedAt = DateTime.UtcNow,
            Detail = $"Ticket {ticket.Id} bị xóa bởi {role}"
        });

        return true;
    }

    // =========================
    // RESTORE + AUDIT
    // =========================
    public async Task<bool> RestoreTicketAsync(Guid ticketId, string role)
    {
        var ticket = await ticketRepo.GetByIdAsync(ticketId);

        if (ticket == null || !ticket.IsDeleted)
            return false;

        if (role != "Admin")
            return false;

        ticket.IsDeleted = false;
        ticket.DeletedAt = null;
        ticket.DeletedBy = null;

        await ticketRepo.UpdateAsync(ticket);

        await auditRepo.AddAsync(new AuditLog
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            Action = "RESTORE_TICKET",
            CreatedAt = DateTime.UtcNow,
            Detail = $"Ticket {ticket.Id} được khôi phục"
        });

        return true;
    }

    // =========================
    // ASSIGN
    // =========================
    public async Task<bool> AssignTicketAsync(Guid ticketId, Guid assignedToId)
    {
        var ticket = await ticketRepo.GetByIdAsync(ticketId);

        if (ticket == null || ticket.IsDeleted) return false;

        ticket.AssignedToId = assignedToId;

        await ticketRepo.UpdateAsync(ticket);

        return true;
    }

    // =========================
    // STATUS
    // =========================
    public async Task<bool> UpdateTicketStatusAsync(Guid ticketId, string newStatus)
    {
        var ticket = await ticketRepo.GetByIdAsync(ticketId);

        if (ticket == null || ticket.IsDeleted) return false;

        var validTransitions = new Dictionary<string, List<string>>
        {
            { "Open", new List<string> { "In Progress" } },
            { "In Progress", new List<string> { "Resolved"} },
            { "Resolved", new List<string> { "Closed" } },
            { "Closed", new List<string>() }
        };

        if (!validTransitions.ContainsKey(ticket.Status) ||
            !validTransitions[ticket.Status].Contains(newStatus))
        {
            throw new InvalidOperationException(
                $"Không thể chuyển trạng thái từ '{ticket.Status}' sang '{newStatus}'.");
        }

        ticket.Status = newStatus;

        await ticketRepo.UpdateAsync(ticket);

        return true;
    }
// =========================
// HIDE TICKET
// =========================
public async Task<bool> HideTicketAsync(Guid ticketId, Guid userId)
{
    var ticket = await ticketRepo.GetByIdAsync(ticketId);

    if (ticket == null || ticket.IsDeleted)
        return false;

    ticket.IsHidden = true;
    ticket.HiddenAt = DateTime.UtcNow;

    await ticketRepo.UpdateAsync(ticket);

    return true;
}

// =========================
// UNHIDE TICKET
// =========================
public async Task<bool> UnhideTicketAsync(Guid ticketId, Guid userId)
{
    var ticket = await ticketRepo.GetByIdAsync(ticketId);

    if (ticket == null || ticket.IsDeleted)
        return false;

    ticket.IsHidden = false;
    ticket.HiddenAt = null;

    await ticketRepo.UpdateAsync(ticket);

    return true;
}
}