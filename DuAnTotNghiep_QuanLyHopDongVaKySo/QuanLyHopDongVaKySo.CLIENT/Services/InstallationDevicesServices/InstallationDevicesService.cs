using Azure;
using Microsoft.IdentityModel.Tokens;
using QuanLyHopDongVaKySo_API.Models;
using System.Net.Http.Headers;

namespace QuanLyHopDongVaKySo.CLIENT.Services.InstallationDevicesServices
{
    public class InstallationDevicesService : IInstallationDevicesService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private string token;

        public InstallationDevicesService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
        public async Task<string> AddNewDevice(InstallationDevice postDevice)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.PostAsJsonAsync("api/InstallationDevices/AddNew", postDevice);
            if (reponse.IsSuccessStatusCode)
            {
                return postDevice.Device_ID.ToString();
            }
            return null;
        }

        public async Task<int> DelectDevice(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.DeleteAsync($"api/InstallationDevices/Delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                return id;
            }
            return 0;
        }

        public async Task<List<InstallationDevice>> GetAllByServiceId(int serviceID)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<List<InstallationDevice>>($"api/InstallationDevices/GetAllByServiceID/{serviceID}");
            return reponse;
        }

        public async Task<List<InstallationDevice>> GetAllDevice()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<List<InstallationDevice>>("api/InstallationDevices");
            return reponse;
        }

        public async Task<InstallationDevice> GetDeviceById(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<InstallationDevice>($"api/InstallationDevices/{id}");
            return reponse;
        }

        public async Task<string> UpdateDevice(InstallationDevice putDevice)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.PutAsJsonAsync("api/InstallationDevices/Update", putDevice);
            if (reponse.IsSuccessStatusCode)
            {
                return putDevice.Device_ID.ToString();
            }
            return null;
        }
    }
}
