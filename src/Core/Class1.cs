namespace Core;

// Định nghĩa các trạng thái
public enum TicketStatus { Open, InProgress, Resolved, Closed }

public class Ticket 
{
    public string Title { get; set; } = "";
    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public string Priority { get; set; } = "Low";
    public string Description { get; set; } = string.Empty;
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }


    public void AssignTo(string staffId) => Status = TicketStatus.InProgress;
    public void MarkAsResolved() => Status = TicketStatus.Resolved;
    public void CloseTicket() => Status = TicketStatus.Closed;
    
    public bool UpdateTitle(string newTitle) {
        if (Status == TicketStatus.Closed) return false;
        Title = newTitle;
        return true;
    }
}