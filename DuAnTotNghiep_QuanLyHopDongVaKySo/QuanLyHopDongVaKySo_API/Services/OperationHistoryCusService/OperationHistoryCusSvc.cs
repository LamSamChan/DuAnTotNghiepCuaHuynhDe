using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.OperationHistoryCusService
{
    public class OperationHistoryCusSvc : IOperationHistoryCusSvc
    {
        private readonly ProjectDbContext _context;

        public OperationHistoryCusSvc(ProjectDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddNew(OperationHistoryCus oHistoryCus)
        {
            int isSuccess = 0;
            _context.OperationHistoryCus.Add(oHistoryCus);
            await _context.SaveChangesAsync();
            isSuccess = oHistoryCus.HistoryID;
            return isSuccess;
        }

        public async Task<List<OperationHistoryCus>> GetAll()
        {
            try
            {
                return await _context.OperationHistoryCus.ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<OperationHistoryCus>();
            }
        }

        public async Task<List<OperationHistoryCus>> GetListById(string customer_ID)
        {
            try
            {
                return await _context.OperationHistoryCus.Where(c => c.CustomerID == Guid.Parse(customer_ID)).ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<OperationHistoryCus>();
            }
        }
    }
}
