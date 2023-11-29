using Azure;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo.CLIENT.Constants;
using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using System.Net.Http;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services
{
   public interface IAuthServices
    {
        Task<string> Login(VMLogin login);
    }
    public class AuthServices : IAuthServices
    {
        private readonly HttpClient _httpClient;
        private readonly IEmployeeService _employeeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthServices(HttpClient httpClient, IEmployeeService employeeService, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _employeeService = employeeService;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<string> Login(VMLogin login)
        {
            string json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PostAsync("api/Auth", content))
            {
                if (response.IsSuccessStatusCode)
                {         
                    return await response.Content.ReadAsStringAsync();
                }
                return null;
            }
        }
    }
}
