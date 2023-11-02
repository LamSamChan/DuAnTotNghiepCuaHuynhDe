using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_CLIENT.Services.EmployeesServices
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployees();
        Task<Employee> GetEmployeeById(string id);
        Task<string> AddNewEmployee(Employee postEmployee);
        Task<string> UpdateEmployee(Employee putEmployee);
    }
}
