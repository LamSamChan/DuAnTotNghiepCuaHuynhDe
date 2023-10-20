using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.OperationHistoryEmpService
{
    public interface IOperationHistoryEmpSvc
    {
        Task<List<OperationHistoryEmp>> GetAll();
        Task<List<OperationHistoryEmp>> GetListById(string emp_ID);
        Task<int> AddNew(OperationHistoryEmp oHistoryEmp);
    }
}
