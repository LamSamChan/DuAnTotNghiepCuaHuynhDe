using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.PositionService
{
    public class PositionSvc : IPositionSvc
    {
        private readonly ProjectDbContext _context;

        public PositionSvc(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddNew(Position position)
        {
            int isSuccess = 0;  
            _context.Positions.Add(position);
            await _context.SaveChangesAsync();
            isSuccess = position.PositionID;
            return isSuccess;
        }

        public async Task<List<Position>> GetAll()
        {
            try
            {
                return await _context.Positions.ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<Position>();
            }
        }

        public async Task<List<Position>> GetAllNotHidden()
        { 
            try
            { 
                var notHiddenList = await _context.Positions.Where(h => !h.isHidden).ToListAsync();
                if (notHiddenList.Count > 0)
                {
                    return notHiddenList;
                }
                else
                {
                    return new List<Position>();
                }
            }
            catch (Exception ex)
            {
                return new List<Position>();
            }
        }

        public async Task<List<Position>> GetAllHidden()
        {
            try
            {
                var notHiddenList = await _context.Positions.Where(h => h.isHidden).ToListAsync();
                if (notHiddenList.Count > 0)
                {
                    return notHiddenList;
                }
                else
                {
                    return new List<Position>();
                }
            }
            catch (Exception ex)
            {
                return new List<Position>();
            }
        }

        public async Task<Position> GetById(int positionId)
        {
            try
            {
                var position = new Position();
                position = _context.Positions.FirstOrDefault(p => p.PositionID == positionId);
                if (position != null)
                {
                    return position;
                }
                else
                {
                    return new Position();
                }
            }
            catch (Exception ex)
            {
                return new Position();
            }
        }

        public async Task<int> Update(Position position)
        {
            int status = 0;
            try
            {
                // Lấy đối tượng từ database dựa vào id
                var existingPostion = _context.Positions.FirstOrDefault(p => p.PositionID == position.PositionID);

                // Kiểm tra xem đối tượng có tồn tại trong database không
                if (existingPostion == null)
                {
                    return 0;
                }

                // Cập nhật thông tin của đối tượng 
                existingPostion.PositionID = position.PositionID;
                existingPostion.PositionName = position.PositionName;
                existingPostion.isHidden = position.isHidden;

                // Lưu thay đổi vào database
                await _context.SaveChangesAsync();
                status = position.PositionID;
            }
            catch (System.Exception ex)
            {
                status = 0;
            }
            return status;
        }
    }
}
