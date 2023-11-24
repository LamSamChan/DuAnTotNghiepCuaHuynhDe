using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.OperationHistoryEmpService
{
    public class OperationHistoryEmpSvc : IOperationHistoryEmpSvc
    {
        private readonly ProjectDbContext _context;

        public OperationHistoryEmpSvc(ProjectDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddNew(OperationHistoryEmp oHistoryEmp)
        {
            int isSuccess = 0;
            _context.OperationHistoryEmp.Add(oHistoryEmp);
            await _context.SaveChangesAsync();
            isSuccess = oHistoryEmp.HistoryID;
            return isSuccess;
        }

        public async Task<List<OperationHistoryEmp>> GetAll()
        {
            try
            {
                return await _context.OperationHistoryEmp.ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<OperationHistoryEmp>();
            }
        }

        public async Task<List<OperationHistoryEmp>> GetListById(string emp_ID)
        {
            try
            {
                return await _context.OperationHistoryEmp.Where(e => e.EmployeeID == Guid.Parse(emp_ID)).ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<OperationHistoryEmp>();
            }
        }
    }
}
