using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Services.HistoryServices
{
    public interface IHistoryCusSvc
    {
        Task<List<OperationHistoryCus>> GetAll();
        Task<List<OperationHistoryCus>> GetListById(string customer_ID);
        Task<int> AddNew(OperationHistoryCus oHistoryCus);
    }
}
