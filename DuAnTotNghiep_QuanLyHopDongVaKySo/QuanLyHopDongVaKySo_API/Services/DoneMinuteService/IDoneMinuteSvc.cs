using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.DoneMinuteService
{
    public interface IDoneMinuteSvc
    {
        Task<List<DoneMinute>> GetAll();
        Task<List<DoneMinute>> GetListByEmpId(string EmployeeId);
        Task<DoneMinute> GetById(int dMinuteId);
        Task<int> AddNew(DoneMinute doneMinute);
        Task<DoneMinute> AddDMinuteFromSignByUSBToken(DoneMinute DoneMinute);
        Task<DoneMinute> UpdateMinuteFromSignByUSBToken(DoneMinute DoneMinute);

    }
}
