using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.RoleService
{
    public interface IRoleSvc
    {
        Task<List<Role>> GetAll();
        Task<List<Role>> GetAllNotHidden();
        Task<List<Role>> GetAllHidden();

        Task<Role> GetById(int roleId);
        Task<int> AddNew(Role role);
        Task<int> Update(Role role);
    }
}
