using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.RoleService
{
    public class RoleSvc : IRoleSvc
    {
        private readonly ProjectDbContext _context;
        public RoleSvc(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddNew(Role role)
        {
            int isSuccess = 0;
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            isSuccess = role.RoleID;
            return isSuccess;
        }

        public async Task<List<Role>> GetAll()
        {
            try
            {
                return await _context.Roles.ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<Role>();
            }
        }

        public async Task<List<Role>> GetAllNotHidden()
        {
            try
            {
               var notHiddenList = await _context.Roles.Where(h => !h.isHidden).ToListAsync();
               if (notHiddenList.Count > 0) {
                    return notHiddenList;
               }
               else
               {
                    return new List<Role>();
                }
            }
            catch (Exception ex)
            {
                return new List<Role>();
            }
        }

        public async Task<List<Role>> GetAllHidden()
        {
            try
            {
                var hiddenList = await _context.Roles.Where(h => h.isHidden).ToListAsync();
                if (hiddenList.Count > 0)
                {
                    return hiddenList;
                }
                else
                {
                    return new List<Role>(); ;
                }
            }
            catch (Exception ex)
            {
                return new List<Role>();
            }
        }

        public async Task<Role> GetById(int roleId)
        {
            try
            {
                return _context.Roles.FirstOrDefault(r => r.RoleID == roleId);
            }
            catch (Exception ex)
            {
                return new Role();
            }
        }

        public async Task<int> Update(Role role)
        {
            int status = 0;
            try
            {
                // Lấy đối tượng từ database dựa vào id
                var existingPostion = _context.Roles.FirstOrDefault(p => p.RoleID == role.RoleID);

                // Kiểm tra xem đối tượng có tồn tại trong database không
                if (existingPostion == null)
                {
                    return 0;
                }

                // Cập nhật thông tin của đối tượng 
                existingPostion.RoleID = role.RoleID;
                existingPostion.RoleName = role.RoleName;
                existingPostion.isHidden = role.isHidden;

                // Lưu thay đổi vào database
                await _context.SaveChangesAsync();
                status = role.RoleID;
            }
            catch (System.Exception ex)
            {
                status = 0;
            }
            return status;
        }
    }
}
