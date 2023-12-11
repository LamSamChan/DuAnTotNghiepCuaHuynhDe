using QuanLyHopDongVaKySo_API.ViewModels;

namespace QuanLyHopDongVaKySo.CLIENT.Services.PasswordServices
{
    public interface IPasswordService
    {
        Task<string> ChangePasswordAsync(ChangePassword changePassword);
        Task<string> ForgotPasswordAsync(string comfirmOTP);
        Task<string> GetOTPChangeAsync(string employeeId);
        Task<string> GetOTPForgotAsync(ForgotPassword forgotPassword);
    }
}
