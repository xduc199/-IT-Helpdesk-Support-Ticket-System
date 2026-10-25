namespace Core.DTOs
{
    public class AssignRoleDto
    {
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Ví dụ: "IT_Support" hoặc "Admin"
    }
}