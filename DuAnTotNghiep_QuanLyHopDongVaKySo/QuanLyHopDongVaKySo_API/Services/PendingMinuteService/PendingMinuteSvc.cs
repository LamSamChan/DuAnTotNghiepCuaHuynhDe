using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.PendingMinuteService
{
    public class PendingMinuteSvc : IPendingMinuteSvc
    {
        private readonly ProjectDbContext _context;

        public PendingMinuteSvc(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<int> DeletePMinute(int pMinuteId)
        {
            int status = 0;
            try
            {
                var pMinute = _context.PendingMinutes.FirstOrDefault(m => m.PendingMinuteId == pMinuteId);
                _context.Remove(pMinute);
                await _context.SaveChangesAsync();
                status = pMinute.PendingMinuteId;
            }
            catch (Exception ex)
            {
                return status;
            }
            return status;
        }

        public async Task<List<PendingMinute>> GetAll()
        {
            try
            {
                return await _context.PendingMinutes.ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<PendingMinute>();
            }
        }

        public async Task<PendingMinute> GetById(int pMinuteId)
        {
            try
            {
                return _context.PendingMinutes.FirstOrDefault(p => p.PendingMinuteId == pMinuteId);
            }
            catch (Exception ex)
            {
                return new PendingMinute();
            }
        }

        public async Task<int> GetJobFormIRequirement(PendingMinute pendingMinute)
        {
            try
            {
                int isSuccess = 0;
                _context.PendingMinutes.Add(pendingMinute);
                await _context.SaveChangesAsync();
                isSuccess = pendingMinute.PendingMinuteId;
                return isSuccess;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<PendingMinute>> GetListByEmpId(string EmployeeId)
        {
            try
            {
                return await _context.PendingMinutes.Where(p => p.EmployeeId == Guid.Parse(EmployeeId)).ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<PendingMinute>();
            }
        }
    }
}
