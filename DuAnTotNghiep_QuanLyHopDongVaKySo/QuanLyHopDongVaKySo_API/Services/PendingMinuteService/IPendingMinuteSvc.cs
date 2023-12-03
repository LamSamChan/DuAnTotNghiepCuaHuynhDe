using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.PendingMinuteService
{
    public interface IPendingMinuteSvc
    {
        Task<List<PendingMinute>> GetAll();
        Task<List<PendingMinute>> GetListByEmpId(string EmployeeId);
        Task<PendingMinute> GetById(int pMinuteId);
        Task<int> DeletePMinute(int pMinuteId);
        Task<int> addAsnyc(PendingMinute pMinute);
        Task<MinuteInfo> ExportMinute(PendingMinute pMinute,string empId);
        Task<int> updatePMinuteFile(int id, string File,string base64String);
        Task<string> updateAsnyc(PutPMinute pMinute);

        Task<PendingMinute> getByIdForWinformAsnyc(int id, string cusId);

    }
}
