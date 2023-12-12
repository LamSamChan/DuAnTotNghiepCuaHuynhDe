using QuanLyHopDongVaKySo_API.Models.ModelProcedure;

namespace QuanLyHopDongVaKySo_API.Services.ExecProcedureService
{
    public interface IExecProcedureServices
    {
        Task<List<CountCustomerAdded>> CountCustomerAddedByDate(DateTime startDate, DateTime endDate);
        Task<List<CountCustomerAdded>> CountCustomerAddedByWeek(DateTime month);
        Task<List<CountCustomerAdded>> CountCustomerAddedByMonth(DateTime month);

    }
}
