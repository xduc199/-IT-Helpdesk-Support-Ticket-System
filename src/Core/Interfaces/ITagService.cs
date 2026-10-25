using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    // DTO hứng dữ liệu trả về cho Frontend
    public class TagResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    // Các Request nhận dữ liệu từ Frontend gửi lên
    public class CreateTagRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateTagRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    // Khai báo các hàm giao tiếp
    public interface ITagService
    {
        Task<List<TagResponseDto>> GetAllTagsAsync();
        Task<TagResponseDto> CreateTagAsync(CreateTagRequest request);
        Task<bool> UpdateTagAsync(Guid id, UpdateTagRequest request);
        Task<string> DeleteTagAsync(Guid id);
    }
}