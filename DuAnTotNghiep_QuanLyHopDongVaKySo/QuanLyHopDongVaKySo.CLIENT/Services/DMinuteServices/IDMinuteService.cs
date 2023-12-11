using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ViewPost;

namespace QuanLyHopDongVaKySo.CLIENT.Services.DMinuteServices
{
    public interface IDMinuteService
    {
        Task<List<DoneMinute>> GetAll();
        Task<List<DoneMinute>> GetListByEmpId(string EmployeeId);
        Task<DoneMinute> GetById(int dMinuteId);
        Task<string> SignMinuteWithUSBToken(PostDMinute_Usb dMinute);

    }
}
