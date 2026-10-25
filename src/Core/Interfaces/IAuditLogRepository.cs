using Core.Entities;

namespace Core.Interfaces;

public interface IAuditLogRepository
{
    Task<IEnumerable<AuditLog>> GetAllAsync();

    Task<IEnumerable<AuditLog>> GetByTicketIdAsync(Guid ticketId);

    Task AddAsync(AuditLog auditLog);
}