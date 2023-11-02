using QuanLyHopDongVaKySo_API.ViewModels;

namespace QuanLyHopDongVaKySo_CLIENT.Services.PasswordServices
{
    public interface IPasswordService
    {
        Task<string> ChangePasswordAsync(ChangePassword changePassword, string employeeId, string comfirmOTP);
        Task<string> ForgotPasswordAsync(string comfirmOTP);
        Task<string> GetOTPChangeAsync(string employeeId);
        Task<string> GetOTPForgotAsync(ForgotPassword forgotPassword);
    }
}
