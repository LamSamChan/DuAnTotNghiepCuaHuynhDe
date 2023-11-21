using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.StampService
{
    public class StampSvc : IStampSvc
    {
        private readonly ProjectDbContext _context;

        public StampSvc(ProjectDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddNew(Stamp stamp)
        {
            int isSuccess = 0;
            _context.Stamps.Add(stamp);
            await _context.SaveChangesAsync();
            isSuccess = stamp.ID;
            return isSuccess;
        }

        public async Task<List<Stamp>> GetAll()
        {
            try
            {
                return await _context.Stamps.ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<Stamp>();
            }
        }

        public async Task<Stamp> GetById(int id)
        {
            try
            {
                var stamp = new Stamp();
                stamp = _context.Stamps.FirstOrDefault(s => s.ID == id);
                if (stamp != null)
                {
                    return stamp;
                }
                else
                {
                    return new Stamp();
                }
            }
            catch (Exception ex)
            {
                return new Stamp();
            }
        }

        public async Task<int> Update(Stamp stamp)
        {
            int status = 0;
            try
            {
                // Lấy đối tượng từ database dựa vào id
                var existingStamp = _context.Stamps.FirstOrDefault(s => s.ID == stamp.ID);

                // Kiểm tra xem đối tượng có tồn tại trong database không
                if (existingStamp == null)
                {
                    return 0;
                }

                // Cập nhật thông tin của đối tượng 
                existingStamp.ID = stamp.ID;
                existingStamp.DateUpdated = DateTime.Now;
                existingStamp.StampPath = stamp.StampPath;

                // Lưu thay đổi vào database
                await _context.SaveChangesAsync();
                status = stamp.ID;
            }
            catch (System.Exception ex)
            {
                status = 0;
            }
            return status;
        }

        public async Task<int> Delete(int id)
        {
            int result = 0;
            var existingStamp = _context.Stamps.FirstOrDefault(s => s.ID == id);
            _context.Stamps.Remove(existingStamp);
            await _context.SaveChangesAsync();
            result = id;
            return result;
        }
    }
}
