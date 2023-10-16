using System;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.TemplateMinuteService
{
    public interface ITemplateMinuteSvc
    {
        Task<TemplateMinute> getTMinuteAsnyc(int id);
        Task<List<TemplateMinute>> getTMinutesAsnyc();
        Task<int> addTMinuteAsnyc(PostTMinute tMinute);
        Task<int> updateTMinuteAsnyc(PutTMinute tMinute);
        Task<bool> deleteTMinueAsnyc(int id);
    }
}
