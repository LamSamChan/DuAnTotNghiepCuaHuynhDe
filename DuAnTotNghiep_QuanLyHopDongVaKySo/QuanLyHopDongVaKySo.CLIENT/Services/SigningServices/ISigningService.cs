using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.ViewModels;

namespace QuanLyHopDongVaKySo.CLIENT.Services.SigningServices
{
    public interface ISigningService
    {
        Task<string> SignContractByDirector(SigningModel signing);
        Task<string> SignMinuteByInstaller(SigningModel signing);
        Task<string> SignContractByCustomer(SigningModel signing);
        Task<string> SignMinuteByCustomer(SigningModel signing);
    }
}
