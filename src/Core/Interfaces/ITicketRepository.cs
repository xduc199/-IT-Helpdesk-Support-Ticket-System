namespace Core.Interfaces;

public interface ITicketRepository
{
    Task<IEnumerable<Core.Entities.Ticket>> GetAllAsync();
    Task<Core.Entities.Ticket?> GetByIdAsync(Guid id);
    Task AddAsync(Core.Entities.Ticket ticket);
    // BỔ SUNG: Hàm update
    Task UpdateAsync(Core.Entities.Ticket ticket); 
}