using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class TagRepository(ApplicationDbContext context) : ITagRepository
{
    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        // Load Tag -> Danh sách Tickets -> Người tạo (Requester) của từng Ticket
        return await context.Tags
            .Include(t => t.Tickets)
                .ThenInclude(tk => tk.Requester)
            .ToListAsync();
    }

    public async Task<Tag?> GetByIdAsync(Guid id)
    {
        return await context.Tags
            .Include(t => t.Tickets)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddAsync(Tag tag)
    {
        await context.Tags.AddAsync(tag);
        await context.SaveChangesAsync();
    }
}