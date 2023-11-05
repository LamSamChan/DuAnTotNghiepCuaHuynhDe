using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo.CLIENT.Models;
using Newtonsoft.Json;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices
{
    public class EmployeesService : IEmployeeService
    {
        private readonly HttpClient _httpClient;
        public EmployeesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> AddNewEmployee(PostEmployee postEmployee)
        {
            string json = JsonConvert.SerializeObject(postEmployee);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PostAsync("api/Employees/AddNew", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return 1;
                }
                return 0;
            }
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            var reponse = await _httpClient.GetFromJsonAsync<List<Employee>>("api/Employees");
            return reponse;

        }

        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            var reponse = await _httpClient.GetFromJsonAsync<Employee>($"api/Employees/GetByEmail/{email}");
            return reponse;
        }

        public async Task<PutEmployee> GetEmployeeById(string id)
        {
            var reponse = await _httpClient.GetFromJsonAsync<PutEmployee>($"api/Employees/{id}");
            return reponse;
        }

        public async Task<string> UpdateEmployee(PutEmployee putEmployee)
        {
            string json = JsonConvert.SerializeObject(putEmployee);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PutAsync("api/Employees/Update", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return putEmployee.EmployeeId.ToString();
                }
                return null;
            }
        }

    }
}
