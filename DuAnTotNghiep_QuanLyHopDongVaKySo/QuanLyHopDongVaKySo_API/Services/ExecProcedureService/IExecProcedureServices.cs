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



        Task<List<CountPendingContractCreadted>> CountPContractWaitCusByDate(DateTime startDate, DateTime endDate);
        Task<List<CountPendingContractCreadted>> CountPContractWaitCusByWeek(DateTime month);
        Task<List<CountPendingContractCreadted>> CountPContractWaitCusByMonth(DateTime month);


        Task<List<CountPendingContractCreadted>> CountDContractByDate(DateTime startDate, DateTime endDate);
        Task<List<CountPendingContractCreadted>> CountDContractByWeek(DateTime month);
        Task<List<CountPendingContractCreadted>> CountDContractByMonth(DateTime month);


        Task<List<CountPendingContractCreadted>> CountUnEffectByDate(DateTime startDate, DateTime endDate);
        Task<List<CountPendingContractCreadted>> CountUnEffectByWeek(DateTime month);
        Task<List<CountPendingContractCreadted>> CountUnEffectByMonth(DateTime month);
    }
}
