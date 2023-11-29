using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using System.Net.Http.Headers;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.RoleServices
{
    public class RoleService : IRoleService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private string token;

        public RoleService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
        public async Task<int> AddRoleAsync(Role role)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
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
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<List<Role>>("api/Roles/NotHidden");
            return reponse;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<List<Role>>("api/Roles");
            return reponse;
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<Role>($"api/Roles/{id}");
            return reponse;
        }

        public async Task<int> UpdateRoleAsync(Role role)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
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
