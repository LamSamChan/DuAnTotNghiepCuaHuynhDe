using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;

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
