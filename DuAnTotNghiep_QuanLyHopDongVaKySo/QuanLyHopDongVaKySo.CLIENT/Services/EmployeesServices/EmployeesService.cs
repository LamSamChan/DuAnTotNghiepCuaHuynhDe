using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices
{
    public class EmployeesService : IEmployeeService
    {
        private readonly HttpClient _httpClient;
        public EmployeesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> AddNewEmployee(Employee postEmployee)
        {
            var reponse = await _httpClient.PostAsJsonAsync("api/Employees/AddNew", postEmployee);
            reponse.EnsureSuccessStatusCode();
            return await reponse.Content.ReadAsStringAsync();
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            var reponse = await _httpClient.GetFromJsonAsync<List<Employee>>("api/Employees");
            return reponse;

        }

        public async Task<Employee> GetEmployeeById(string id)
        {
            var reponse = await _httpClient.GetFromJsonAsync<Employee>($"api/Employees/{id}");
            return reponse;
        }

        public async Task<string> UpdateEmployee(Employee putEmployee)
        {
            var reponse = await _httpClient.PutAsJsonAsync("api/Employees/Update", putEmployee);
            reponse.EnsureSuccessStatusCode();
            return await reponse.Content.ReadAsStringAsync();
        }
    }
}
