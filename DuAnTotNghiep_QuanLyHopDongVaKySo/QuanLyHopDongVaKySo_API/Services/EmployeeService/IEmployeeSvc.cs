using QuanLyHopDongVaKySo_API.Models;
using System.Threading.Tasks;

namespace QuanLyHopDongVaKySo_API.Services.EmployeeService
{
    public interface IEmployeeSvc
    {
        Task<List<Employee>> GetAll();
        Task<Employee> GetById(string empID);
        Task<string> AddNew(Employee employee);
        Task<string> Update(Employee employee);
        Task<string> IsFieldExist(Employee employee);
        Task<string> IsRoleOrPositonCheck(Employee employee);
    }
}
