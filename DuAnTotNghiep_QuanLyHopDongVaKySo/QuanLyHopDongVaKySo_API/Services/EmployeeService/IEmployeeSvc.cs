using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.ViewModels;
using System.Threading.Tasks;

namespace QuanLyHopDongVaKySo_API.Services.EmployeeService
{
    public interface IEmployeeSvc
    {
        Task<List<Employee>> GetAll();
        Task<Employee> GetById(string empID);
        Task<Employee> GetByEmail(string empEmail);
        Task<string> AddNew(Employee employee);
        Task<string> Update(Employee employee);
        Task<string> IsFieldExist(Employee employee);
        Task<string> IsRoleOrPositonCheck(Employee employee);
        Task<Employee> Login(ViewLogin viewLogin);
        Task<int?> ChangePassword(string empId, ChangePassword changePassword);
        Task<int> ForgotPassword(string newPassword, ForgotPassword forgotPassword);
    }
}
