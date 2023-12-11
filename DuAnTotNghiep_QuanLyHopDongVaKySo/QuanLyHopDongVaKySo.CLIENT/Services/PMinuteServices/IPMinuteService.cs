using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ViewPost;

namespace QuanLyHopDongVaKySo.CLIENT.Services.PMinuteServices
{
    public interface IPMinuteService
    {
        Task<List<PendingMinute>> GetAll();
        Task<PendingMinute> GetById(int pMinuteId);

        Task<List<PendingMinute>> GetByEmpId(string id);

        Task<string> GetTaskFormIRequirement(PostGetTaskFromIR task);
    }
}
