using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo.CLIENT.Models;
namespace QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployees();
        Task<PutEmployee> GetEmployeePutById(string id);
        Task<Employee> GetEmployeeById(string id);
        Task<Employee> GetEmployeeByEmail(string email);
        Task<int> AddNewEmployee(PostEmployee postEmployee);
        Task<string> UpdateEmployee(PutEmployee putEmployee);
    }
}
