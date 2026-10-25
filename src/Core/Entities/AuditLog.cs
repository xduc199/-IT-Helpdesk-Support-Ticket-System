namespace Core.Entities;

public class AuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? UserId { get; set; }

    public string Action { get; set; } = string.Empty; 
    // DELETE_TICKET, RESTORE_TICKET

    public Guid? TicketId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string Detail { get; set; } = string.Empty;
}