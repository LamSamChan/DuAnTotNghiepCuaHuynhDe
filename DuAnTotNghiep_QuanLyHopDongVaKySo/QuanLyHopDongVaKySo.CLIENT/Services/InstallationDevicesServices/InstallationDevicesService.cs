using Azure;
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
            if (reponse.IsSuccessStatusCode)
            {
                return postDevice.Device_ID.ToString();
            }
            return null;
        }

        public async Task<int> DelectDevice(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/InstallationDevices/Delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                return id;
            }
            return 0;
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
            var reponse = await _httpClient.PutAsJsonAsync("api/InstallationDevices/Update", putDevice);
            if (reponse.IsSuccessStatusCode)
            {
                return putDevice.Device_ID.ToString();
            }
            return null;
        }
    }
}
