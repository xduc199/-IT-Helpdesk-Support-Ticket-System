using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces;

public interface ITicketService
{
    Task<TicketDto> CreateTicketAsync(CreateTicketDto dto, Guid requesterId);

    Task<(IEnumerable<TicketDto> Tickets, int TotalCount)> GetAllTicketsAsync(int pageNumber, int pageSize);

    Task<TicketDto?> GetTicketByIdAsync(Guid id);

    Task<bool> AssignTicketAsync(Guid ticketId, Guid assignedToId);

    Task<bool> UpdateTicketStatusAsync(Guid id, string status);

    Task<bool> DeleteTicketAsync(Guid ticketId, string role, Guid userId);

    Task<bool> RestoreTicketAsync(Guid ticketId, string role);
    
    Task<bool> HideTicketAsync(Guid ticketId, Guid userId);
    Task<bool> UnhideTicketAsync(Guid ticketId, Guid userId);
    
}