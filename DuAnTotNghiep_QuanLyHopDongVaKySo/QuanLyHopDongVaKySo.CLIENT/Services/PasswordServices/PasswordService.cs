using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.ViewModels;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.PasswordServices
{
    public class PasswordService : IPasswordService
    {
        private readonly HttpClient _httpClient;
        public PasswordService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> ChangePasswordAsync(ChangePassword changePassword, string employeeId, string comfirmOTP)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(changePassword), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"api/Password/ChangePassword?employeeId={employeeId}&comfirmOTP={comfirmOTP}", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                   // Chưa có code xử lý lỗi
                    return "Lỗi khi thực hiện cuộc gọi API.";
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có ngoại lệ
                return "Lỗi khi thực hiện cuộc gọi API: " + ex.Message;
            }
        }

        public async Task<string> ForgotPasswordAsync(string comfirmOTP)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/Password/ForgotPassword?comfirmOTP={comfirmOTP}", null);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    // Chưa có code xử lý lỗi
                    return "Lỗi khi thực hiện cuộc gọi API.";
                }
            }
            catch (Exception ex)
            {
                return "Lỗi khi thực hiện cuộc gọi API: " + ex.Message;
            }
        }

        public async Task<string> GetOTPChangeAsync(string employeeId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/Password/GetOTPChange?employeeId={employeeId}", null);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    // Code xử lý lỗi chưa có
                    return "Lỗi khi thực hiện cuộc gọi API.";
                }
            }
            catch (Exception ex)
            {
                return "Lỗi khi thực hiện cuộc gọi API: " + ex.Message;
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
                    return "Lỗi khi thực hiện cuộc gọi API.";
                }
            }
            catch (Exception ex)
            {

                return "Lỗi khi thực hiện cuộc gọi API: " + ex.Message;
            }
        }
    }
}  

