namespace Core.DTOs;
public class TicketDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string RequesterName { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
}