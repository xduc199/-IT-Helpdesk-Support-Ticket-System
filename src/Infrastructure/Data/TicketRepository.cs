using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class TicketRepository(ApplicationDbContext context) : ITicketRepository
{
    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        // Sử dụng AsNoTracking để tăng hiệu năng cho các truy vấn chỉ đọc (GET)
        return await context.Tickets
            .AsNoTracking()
            .Include(t => t.Requester)
            .Include(t => t.Tags)
            .ToListAsync();
    }

    public async Task<Ticket?> GetByIdAsync(Guid id)
    {
        return await context.Tickets
            .Include(t => t.Requester)
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddAsync(Ticket ticket)
    {
        // Xử lý các Tag liên kết trước khi thêm Ticket
        if (ticket.Tags != null && ticket.Tags.Any())
        {
            foreach (var tag in ticket.Tags)
            {
                // Báo cho EF Core rằng các Tag này đã tồn tại trong DB, chỉ tạo liên kết Many-to-Many
                context.Tags.Attach(tag); 
            }
        }

        await context.Tickets.AddAsync(ticket);
        
        try 
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            // Log chi tiết lỗi database để sếp dễ dàng debug trong Terminal
            Console.WriteLine($"[Database Error] Khong the luu Ticket: {ex.InnerException?.Message ?? ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[System Error] Loi khong xac dinh: {ex.Message}");
            throw;
        }
    }
    // Thêm hàm này vào dưới cùng của class TicketRepository
public async Task UpdateAsync(Ticket ticket)
{
    context.Tickets.Update(ticket);
    try 
    {
        await context.SaveChangesAsync();
    }
    catch (DbUpdateException ex)
    {
        Console.WriteLine($"[Database Error] Khong the cap nhat Ticket: {ex.InnerException?.Message ?? ex.Message}");
        throw;
    }
}
}