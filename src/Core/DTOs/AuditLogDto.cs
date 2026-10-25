namespace Core.DTOs;

public class AuditLogDto
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string Action { get; set; } = string.Empty;

    public Guid? TicketId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Detail { get; set; } = string.Empty;
}