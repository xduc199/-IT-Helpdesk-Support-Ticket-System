using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Ticket
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid RequesterId { get; set; }
    public User? Requester { get; set; }

    public Guid? AssignedToId { get; set; }
    public User? AssignedTo { get; set; }

    public ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }

    public bool IsHidden { get; set; } = false;
    public DateTime? HiddenAt { get; set; }
}