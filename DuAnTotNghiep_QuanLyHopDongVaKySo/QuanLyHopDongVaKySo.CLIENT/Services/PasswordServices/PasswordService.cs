using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.ViewModels;
using System.Net.Http.Headers;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.PasswordServices
{
    public class PasswordService : IPasswordService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private string token;

        public PasswordService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task<string> ChangePasswordAsync(ChangePassword changePassword)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(changePassword), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"api/Password/ChangePassword", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    // Chưa có code xử lý lỗi
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có ngoại lệ
                return null;
            }
        }

        public async Task<string> ForgotPasswordAsync(string comfirmOTP)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(comfirmOTP), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"api/Password/ForgotPassword", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    // Chưa có code xử lý lỗi
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> GetOTPChangeAsync(string employeeId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(employeeId), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"api/Password/GetOTPChange", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    // Code xử lý lỗi chưa có
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> GetOTPForgotAsync(ForgotPassword forgotPassword)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(forgotPassword), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"api/Password/GetOTPForgot", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    // Chưa có code xử lý lỗi
                    return null;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}  

