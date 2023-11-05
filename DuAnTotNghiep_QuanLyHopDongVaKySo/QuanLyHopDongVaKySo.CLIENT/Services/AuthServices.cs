﻿using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using System.Net.Http;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services
{
   public interface IAuthServices
    {
        Task<Employee> Login(VMLogin login);
    }
    public class AuthServices : IAuthServices
    {
        private readonly HttpClient _httpClient;
        private readonly IEmployeeService _employeeService;
        public AuthServices(HttpClient httpClient, IEmployeeService employeeService)
        {
            _httpClient = httpClient;
            _employeeService = employeeService;
        }
        public async Task<Employee> Login(VMLogin login)
        {
            string json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PostAsync("api/Auth", content))
            {
                if (response.IsSuccessStatusCode)
                {         
                    return await _employeeService.GetEmployeeByEmail(login.Email);
                }
                return null;
            }
        }
    }
}
