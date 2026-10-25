using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext _context;

        public TagService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Lấy danh sách toàn bộ Tags
        public async Task<List<TagResponseDto>> GetAllTagsAsync()
        {
            return await _context.Tags
                .Select(t => new TagResponseDto { Id = t.Id, Name = t.Name })
                .ToListAsync();
        }

        // 2. Thêm mới Tag (Có check trùng tên)
        public async Task<TagResponseDto> CreateTagAsync(CreateTagRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new Exception("Tên thẻ không được để trống.");

            var isExist = await _context.Tags.AnyAsync(t => t.Name.ToLower() == request.Name.ToLower());
            if (isExist)
                throw new Exception("Tên thẻ này đã tồn tại.");

            var newTag = new Core.Entities.Tag { Id = Guid.NewGuid(), Name = request.Name };
            
            _context.Tags.Add(newTag);
            await _context.SaveChangesAsync();

            return new TagResponseDto { Id = newTag.Id, Name = newTag.Name };
        }

        // 3. Cập nhật sửa tên Tag
        public async Task<bool> UpdateTagAsync(Guid id, UpdateTagRequest request)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return false;

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new Exception("Tên thẻ không được để trống.");

            tag.Name = request.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        // 4. Xóa Tag an toàn dựa trên cơ chế bắt lỗi ngoại lệ từ Database
        public async Task<string> DeleteTagAsync(Guid id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return "NotFound";

            try
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
                return "Success";
            }
            catch (DbUpdateException)
            {
                // Nếu DB báo lỗi vi phạm khóa ngoại (Tag đang được gán vào Ticket nào đó), hệ thống sẽ nhảy vào đây
                return "InUse";
            }
        }
    }
}