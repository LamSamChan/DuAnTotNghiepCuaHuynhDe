using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo.CLIENT.Models;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using QuanLyHopDongVaKySo.CLIENT.Constants;
using Newtonsoft.Json.Linq;

namespace QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices
{
    public class EmployeesService : IEmployeeService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        
        private  string token;

        public EmployeesService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public string Token
        {
            get
            {
                if (!String.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("token")))
                {
                    token = _httpContextAccessor.HttpContext.Session.GetString("token");

                }
                return token;
            }
            set { this.token = value; }
        }

        public async Task<int> AddNewEmployee(PostEmployee postEmployee)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
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
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<List<Employee>>("api/Employees");
            return reponse;

        }

        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<Employee>($"api/Employees/GetByEmail/{email}");
            return reponse;
        }

        public async Task<Employee> GetEmployeeById(string id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<Employee>($"api/Employees/{id}");
            return reponse;
        }

        public async Task<PutEmployee> GetEmployeePutById(string id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<Employee>($"api/Employees/{id}");
            PutEmployee employee = new PutEmployee() { 
                EmployeeId = reponse.EmployeeId,
                FullName = reponse.FullName,
                Email = reponse.Email,
                DateOfBirth = reponse.DateOfBirth,
                Gender = reponse.Gender,
                PhoneNumber = reponse.PhoneNumber,
                Identification = reponse.Identification,
                Image = reponse.Image,
                Address = reponse.Address,
                IsLocked = reponse.IsLocked,
                IsFirstLogin = reponse.IsFirstLogin,
                Note = reponse.Note,
                PositionID = reponse.PositionID,
                RoleID = reponse.RoleID,
            };
            return employee;
        }

        public async Task<string> UpdateEmployee(PutEmployee putEmployee)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
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
