namespace Core.Entities;

public class Tag
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public ICollection<Ticket> Tickets { get; set; } = []; // N-N ngược lại với Ticket
}