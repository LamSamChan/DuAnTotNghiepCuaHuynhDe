using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.PendingMinuteService
{
    public interface IPendingMinuteSvc
    {
        Task<List<PendingMinute>> GetAll();
        Task<List<PendingMinute>> GetListByEmpId(string EmployeeId);
        Task<PendingMinute> GetById(int pMinuteId);
        Task<int> DeletePMinute(int pMinuteId);
        Task<int> GetJobFormIRequirement(PendingMinute pendingMinute);
        Task<string> addAsnyc(PostPMinute pMinute);
        Task<MinuteInfo> ExportContract(PendingMinute pMinute,string empId);
        Task<int> updatePMinuteFile(int id, string File);
    }
}
