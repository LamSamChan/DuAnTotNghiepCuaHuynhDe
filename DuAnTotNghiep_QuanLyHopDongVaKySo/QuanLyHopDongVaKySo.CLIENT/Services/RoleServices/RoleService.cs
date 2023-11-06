using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.RoleServices
{
    public class RoleService : IRoleService
    {
        private readonly HttpClient _httpClient;
        public RoleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> AddRoleAsync(Role role)
        {
            string json = JsonConvert.SerializeObject(role);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                using (var reponse = await _httpClient.PostAsync("api/Roles/AddNew", content))
                {
                   
                    if (reponse.IsSuccessStatusCode)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }

            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<Role>> GetAllNotHidden()
        {
            var reponse = await _httpClient.GetFromJsonAsync<List<Role>>("api/Roles/NotHidden");
            return reponse;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            var reponse = await _httpClient.GetFromJsonAsync<List<Role>>("api/Roles");
            return reponse;
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            var reponse = await _httpClient.GetFromJsonAsync<Role>($"api/Roles/{id}");
            return reponse;
        }

        public async Task<int> UpdateRoleAsync(Role role)
        {
            string json = JsonConvert.SerializeObject(role);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var reponse = await _httpClient.PutAsync("api/Roles/Update", content);
                if (reponse.IsSuccessStatusCode)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
