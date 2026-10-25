using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Core.Interfaces;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
//GIỮ NGUYÊN phong cách Primary Constructor nhưng đổi sang inject ITagService
public class TagsController(ITagService tagService) : ControllerBase
{
    // 1. GET: api/tags
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TagResponseDto>>> GetAll()
    {
        var tags = await tagService.GetAllTagsAsync();
        return Ok(tags);
    }

    // 2. POST: api/tags
    [HttpPost]
    public async Task<ActionResult<TagResponseDto>> Create(CreateTagRequest request)
    {
        try
        {
            var newTag = await tagService.CreateTagAsync(request);
            // Trả về đúng chuẩn RESTful kèm thông tin vật thể vừa tạo
            return CreatedAtAction(nameof(GetAll), new { id = newTag.Id }, newTag);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // 3. PUT: api/tags/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTagRequest request)
    {
        try
        {
            var result = await tagService.UpdateTagAsync(id, request);
            if (!result) return NotFound(new { message = "Không tìm thấy thẻ cần chỉnh sửa." });
            return Ok(new { message = "Cập nhật thẻ thành công!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // 4. DELETE: api/tags/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await tagService.DeleteTagAsync(id);
        
        if (result == "NotFound") return NotFound(new { message = "Không tìm thấy thẻ cần xóa." });
        
        // Trả về thông báo lỗi trực quan nếu thẻ đang được dùng ở bảng Ticket, chống sập DB
        if (result == "InUse") return BadRequest(new { message = "Thẻ này đang được gán cho Ticket lỗi, không thể xóa!" });
        
        return Ok(new { message = "Xóa thẻ thành công khỏi hệ thống!" });
    }
}