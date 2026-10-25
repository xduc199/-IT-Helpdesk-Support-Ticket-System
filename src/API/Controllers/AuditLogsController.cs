using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AuditLogsController(
    IAuditLogRepository repository)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AuditLogDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetAll()
    {
        var logs = await repository.GetAllAsync();

        var result = logs.Select(x => new AuditLogDto
        {
            Id = x.Id,
            UserId = x.UserId,
            Action = x.Action,
            TicketId = x.TicketId,
            CreatedAt = x.CreatedAt,
            Detail = x.Detail
        });

        return Ok(result);
    }

    [HttpGet("ticket/{ticketId}")]
    [ProducesResponseType(typeof(IEnumerable<AuditLogDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetByTicket(Guid ticketId)
    {
        var logs = await repository.GetByTicketIdAsync(ticketId);

        var result = logs.Select(x => new AuditLogDto
        {
            Id = x.Id,
            UserId = x.UserId,
            Action = x.Action,
            TicketId = x.TicketId,
            CreatedAt = x.CreatedAt,
            Detail = x.Detail
        });

        return Ok(result);
    }
}