using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Services.RoleServices
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllRolesAsync();
        Task<List<Role>> GetAllNotHidden();
        Task<Role> GetRoleByIdAsync(int roleId);
        Task<int> AddRoleAsync(Role role);
        Task<int> UpdateRoleAsync(Role role);
    }
}
