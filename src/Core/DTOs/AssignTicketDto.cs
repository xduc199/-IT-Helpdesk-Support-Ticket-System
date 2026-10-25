using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Core.DTOs;

public class AssignTicketDto
{
    [Required]
    [Description("Id kỹ thuật viên nhận xử lý")]
    public Guid AssignedToId { get; set; }
}