using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.OperationHistoryCusService
{
    public interface IOperationHistoryCusSvc
    {
        Task<List<OperationHistoryCus>> GetAll();
        Task<List<OperationHistoryCus>> GetListById(string customer_ID);
        Task<int> AddNew(OperationHistoryCus oHistoryCus);
    }
}
