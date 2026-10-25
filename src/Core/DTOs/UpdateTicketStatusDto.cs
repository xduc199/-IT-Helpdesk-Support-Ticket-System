using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Core.DTOs;

public class UpdateTicketStatusDto
{
    [Required]
    [Description("Open, InProgress, Resolved, Closed")]
    public string Status { get; set; } = string.Empty;
}