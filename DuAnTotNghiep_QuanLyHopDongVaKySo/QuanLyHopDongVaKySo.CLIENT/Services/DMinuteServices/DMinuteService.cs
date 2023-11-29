﻿using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.DMinuteServices
{
    public class DMinuteService : IDMinuteService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private string token;

        public DMinuteService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<DoneMinute>> GetListByEmpId(string EmployeeId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<DoneMinute>>($"api/DMinute/Employee/{EmployeeId}");
            return response;
        }

        public async Task<List<DoneMinute>> GetAll()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<DoneMinute>>("api/DMinute");
            return response;
        }

        public async Task<DoneMinute> GetById(int dMinuteId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<DoneMinute>($"api/DMinute/{dMinuteId}");
            return response;
        }
    }
}
