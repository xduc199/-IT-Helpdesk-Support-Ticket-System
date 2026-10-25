using System.Threading.Tasks;
using Core.DTOs;

namespace Core.Interfaces
{
    public interface IAuthService
    {
        Task<bool> AssignRoleAsync(AssignRoleDto dto);
    }
}