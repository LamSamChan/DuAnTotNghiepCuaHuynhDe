using Microsoft.IdentityModel.Tokens;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Services.InstallationDevicesServices
{
    public class InstallationDevicesService : IInstallationDevicesService
    {
        private readonly HttpClient _httpClient;
        public InstallationDevicesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> AddNewDevice(InstallationDevice postDevice)
        {
            var reponse = await _httpClient.PostAsJsonAsync("api/InstallationDevices/AddNew", postDevice);
            reponse.EnsureSuccessStatusCode();
            return await reponse.Content.ReadAsStringAsync();
        }

        public async Task<int> DelectDevice(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/InstallationDevices/Delete/{id}");
            return 1;
        }

        public async Task<List<InstallationDevice>> GetAllByServiceId(int serviceID)
        {
            var reponse = await _httpClient.GetFromJsonAsync<List<InstallationDevice>>($"api/InstallationDevices/GetAllByServiceID/{serviceID}");
            return reponse;
        }

        public async Task<List<InstallationDevice>> GetAllDevice()
        {
            var reponse = await _httpClient.GetFromJsonAsync<List<InstallationDevice>>("api/InstallationDevices");
            return reponse;
        }

        public async Task<InstallationDevice> GetDeviceById(int id)
        {
            var reponse = await _httpClient.GetFromJsonAsync<InstallationDevice>($"api/InstallationDevices/{id}");
            return reponse;
        }

        public async Task<string> UpdateDevice(InstallationDevice putDevice)
        {
            var reponse = await _httpClient.PostAsJsonAsync("api/InstallationDevices/Update", putDevice);
            reponse.EnsureSuccessStatusCode();
            return await reponse.Content.ReadAsStringAsync();
        }
    }
}
