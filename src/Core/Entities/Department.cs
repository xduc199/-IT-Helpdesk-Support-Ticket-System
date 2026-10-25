namespace Core.Entities;

public class Department
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public ICollection<User> Users { get; set; } = []; // 1-N với User
}