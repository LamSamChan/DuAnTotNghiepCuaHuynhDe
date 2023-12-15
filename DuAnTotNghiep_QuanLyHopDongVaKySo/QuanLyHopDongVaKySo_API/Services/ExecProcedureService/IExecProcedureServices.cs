using QuanLyHopDongVaKySo_API.Models.ModelProcedure;

namespace QuanLyHopDongVaKySo_API.Services.ExecProcedureService
{
    public interface IExecProcedureServices
    {
        Task<List<CountCustomerAdded>> CountCustomerAddedByDate(DateTime startDate, DateTime endDate);
        Task<List<CountCustomerAdded>> CountCustomerAddedByWeek(DateTime month);
        Task<List<CountCustomerAdded>> CountCustomerAddedByMonth(DateTime month);


        Task<List<CountPendingContractCreadted>> CountPContractByDate(DateTime startDate, DateTime endDate);
        Task<List<CountPendingContractCreadted>> CountPContractByWeek(DateTime month);
        Task<List<CountPendingContractCreadted>> CountPContractByMonth(DateTime month);

    }
}
