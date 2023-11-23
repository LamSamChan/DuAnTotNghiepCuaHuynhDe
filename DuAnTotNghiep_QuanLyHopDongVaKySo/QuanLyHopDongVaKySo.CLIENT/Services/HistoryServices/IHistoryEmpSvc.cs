using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Services.HistoryServices
{
    public interface IHistoryEmpSvc
    {
        Task<List<OperationHistoryEmp>> GetAll();
        Task<List<OperationHistoryEmp>> GetListById(string emp_ID);
        Task<int> AddNew(OperationHistoryEmp oHistoryEmp);
    }
}
