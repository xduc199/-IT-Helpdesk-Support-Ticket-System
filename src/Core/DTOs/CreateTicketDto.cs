using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs;

public class CreateTicketDto
{
    [Required]
    [Description("Tiêu đề ticket")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Description("Mô tả sự cố")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Description("Danh sách Tag Id")]
    public List<Guid> TagIds { get; set; } = new();
}