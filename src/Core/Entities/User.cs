using Core.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = "User";

    public bool IsActive { get; set; } = true;

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public ICollection<Ticket> CreatedTickets { get; set; } = new List<Ticket>();
    public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
}