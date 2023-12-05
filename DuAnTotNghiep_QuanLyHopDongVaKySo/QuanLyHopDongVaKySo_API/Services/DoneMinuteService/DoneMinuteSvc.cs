
using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.DoneMinuteService
{
    public class DoneMinuteSvc : IDoneMinuteSvc
    {
        private readonly ProjectDbContext _context;

        public DoneMinuteSvc(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<DoneMinute> AddDMinuteFromSignByUSBToken(DoneMinute doneMinute)
        {
            try
            {
                _context.DoneMinutes.Add(doneMinute);
                await _context.SaveChangesAsync();
                return doneMinute;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<int> AddNew(DoneMinute doneMinute)
        {
            int isSuccess = 0;
            try
            {
                _context.DoneMinutes.Add(doneMinute);
                await _context.SaveChangesAsync();
                isSuccess = doneMinute.DoneMinuteID;
                return isSuccess;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<DoneMinute>> GetAll()
        {
            try
            {
                return await _context.DoneMinutes.ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<DoneMinute>();
            }
        }

        public async Task<DoneMinute> GetById(int dMinuteId)
        {
            try
            {
                return _context.DoneMinutes.FirstOrDefault(d => d.DoneMinuteID == dMinuteId);
            }
            catch (Exception ex)
            {
                return new DoneMinute();
            }
        }


        public async Task<List<DoneMinute>> GetListByEmpId(string EmployeeId)
        {
            try
            {
                return await _context.DoneMinutes.Where(e => e.EmployeeId == Guid.Parse(EmployeeId)).ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<DoneMinute>();
            }
        }

        public async Task<DoneMinute> UpdateMinuteFromSignByUSBToken(DoneMinute doneMinute)
        {
            try
            {
                _context.DoneMinutes.Update(doneMinute);
                await _context.SaveChangesAsync();
                return doneMinute;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
