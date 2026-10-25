using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Core.DTOs;
using Core.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using API.Hubs;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TicketsController(ITicketService ticketService, IHubContext<TicketHub> hubContext)
    : ControllerBase
{
    private readonly IHubContext<TicketHub> _hubContext = hubContext;
    private readonly ITicketService _ticketService = ticketService;

    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<TicketDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResultDto<TicketDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var (tickets, totalCount) =
            await _ticketService.GetAllTicketsAsync(pageNumber, pageSize);

        return Ok(new PagedResultDto<TicketDto>
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Data = tickets.ToList()
        });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketDto>> GetById(Guid id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);

        if (ticket == null)
            return NotFound();

        return Ok(ticket);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TicketDto>> Create([FromBody] CreateTicketDto dto)
    {
        var userIdStr =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
            User.FindFirst("id")?.Value;

        if (string.IsNullOrEmpty(userIdStr))
            return Unauthorized();

        var ticket =
            await _ticketService.CreateTicketAsync(dto, Guid.Parse(userIdStr));

        return Ok(ticket);
    }

    [HttpPut("{id}/assign")]
    [ProducesResponseType(typeof(ApiMessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiMessageDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiMessageDto>> AssignTicket(
        Guid id,
        [FromBody] AssignTicketDto dto)
    {
        var result =
            await _ticketService.AssignTicketAsync(id, dto.AssignedToId);

        if (!result)
        {
            return NotFound(new ApiMessageDto
            {
                Message = "Không tìm thấy Ticket."
            });
        }

        // ================= REAL-TIME =================
        await _hubContext.Clients.All.SendAsync(
            "TicketAssigned",
            new
            {
                TicketId = id,
                AssignedToId = dto.AssignedToId
            }
        );
        // ============================================

        return Ok(new ApiMessageDto
        {
            Message = "Phân công thành công!"
        });
    }

    [HttpPut("{id}/status")]
    [ProducesResponseType(typeof(ApiMessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiMessageDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiMessageDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiMessageDto>> UpdateStatus(
        Guid id,
        [FromBody] UpdateTicketStatusDto dto)
    {
        try
        {
            var result =
                await _ticketService.UpdateTicketStatusAsync(id, dto.Status);

            if (!result)
            {
                return NotFound(new ApiMessageDto
                {
                    Message = "Không tìm thấy Ticket."
                });
            }

            // ================= REAL-TIME =================
            await _hubContext.Clients.All.SendAsync(
                "TicketUpdated",
                new
                {
                    TicketId = id,
                    Status = dto.Status
                }
            );
            // ============================================

            return Ok(new ApiMessageDto
            {
                Message = $"Đã cập nhật trạng thái: {dto.Status}"
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiMessageDto
            {
                Message = ex.Message
            });
        }
        catch
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiMessageDto
                {
                    Message = "Lỗi xử lý server"
                });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiMessageDto>> DeleteTicket(Guid id)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        var userIdStr =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
            User.FindFirst("id")?.Value;

        if (string.IsNullOrEmpty(userIdStr))
            return Unauthorized();

        var userId = Guid.Parse(userIdStr);

        if (role != "Admin" && role != "Technician")
            return Forbid();

        var result =
            await _ticketService.DeleteTicketAsync(id, role!, userId);

        if (!result)
        {
            return NotFound(new ApiMessageDto
            {
                Message = "Không tìm thấy Ticket hoặc đã bị xóa."
            });
        }

        // ================= REAL-TIME =================
        await _hubContext.Clients.All.SendAsync(
            "TicketDeleted",
            new { TicketId = id }
        );
        // ============================================

        return Ok(new ApiMessageDto
        {
            Message = "Xóa ticket thành công (soft delete)."
        });
    }

    [HttpPut("{id}/restore")]
    public async Task<ActionResult<ApiMessageDto>> RestoreTicket(Guid id)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (role != "Admin")
            return Forbid();

        var result =
            await _ticketService.RestoreTicketAsync(id, role!);

        if (!result)
        {
            return NotFound(new ApiMessageDto
            {
                Message = "Không tìm thấy ticket hoặc chưa bị xóa."
            });
        }

        // ================= REAL-TIME =================
        await _hubContext.Clients.All.SendAsync(
            "TicketRestored",
            new { TicketId = id }
        );
        // ============================================

        return Ok(new ApiMessageDto
        {
            Message = "Khôi phục ticket thành công."
        });
    }

    [HttpPut("{id}/hide")]
    public async Task<ActionResult<ApiMessageDto>> Hide(Guid id)
    {
        var userIdStr =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdStr))
            return Unauthorized();

        var result =
            await _ticketService.HideTicketAsync(
                id,
                Guid.Parse(userIdStr));

        if (!result)
        {
            return BadRequest(new ApiMessageDto
            {
                Message = "Không thể ẩn ticket"
            });
        }

        // ================= REAL-TIME =================
        await _hubContext.Clients.All.SendAsync(
            "TicketHidden",
            new { TicketId = id }
        );
        // ============================================

        return Ok(new ApiMessageDto
        {
            Message = "Đã ẩn ticket"
        });
    }

    [HttpPut("{id}/unhide")]
    public async Task<ActionResult<ApiMessageDto>> Unhide(Guid id)
    {
        var userIdStr =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdStr))
            return Unauthorized();

        var result =
            await _ticketService.UnhideTicketAsync(
                id,
                Guid.Parse(userIdStr));

        if (!result)
        {
            return BadRequest(new ApiMessageDto
            {
                Message = "Không thể bỏ ẩn ticket"
            });
        }

        // ================= REAL-TIME =================
        await _hubContext.Clients.All.SendAsync(
            "TicketUnhidden",
            new { TicketId = id }
        );
        // ============================================

        return Ok(new ApiMessageDto
        {
            Message = "Đã bỏ ẩn ticket"
        });
    }
}