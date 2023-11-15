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
        Task<Employee> GetBySerialPFX(string serial);

        Task<string> AddNew(Employee employee);
        Task<string> Update(Employee employee);
        Task<string> IsFieldExist(Employee employee);
        Task<string> IsRoleOrPositonCheck(Employee employee);
        Task<int?> ChangePassword(ChangePassword changePassword);
        Task<int> ForgotPassword(string newPassword, string empEmail);
    }
}
