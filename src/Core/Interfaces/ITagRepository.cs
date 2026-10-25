using Core.Entities;

namespace Core.Interfaces;

public interface ITagRepository
{
    Task<IEnumerable<Tag>> GetAllAsync();
    Task<Tag?> GetByIdAsync(Guid id);
    Task AddAsync(Tag tag);
}