using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AuditLogRepository(ApplicationDbContext context)
    : IAuditLogRepository
{
    public async Task<IEnumerable<AuditLog>> GetAllAsync()
    {
        return await context.AuditLogs
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditLog>> GetByTicketIdAsync(Guid ticketId)
    {
        return await context.AuditLogs
            .AsNoTracking()
            .Where(x => x.TicketId == ticketId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(AuditLog auditLog)
    {
        await context.AuditLogs.AddAsync(auditLog);
        await context.SaveChangesAsync();
    }
}