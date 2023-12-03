using System;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.TemplateMinuteService
{
    public interface ITemplateMinuteSvc
    {
        Task<TemplateMinute> getByIdAsnyc(int id);
        Task<List<TemplateMinute>> getAllAsnyc();
        Task<int> addAsnyc(PostTMinute tMinute);
        Task<int> updateAsnyc(PutTMinute tMinute);
        Task<bool> deleteAsnyc(int id);

    }
}
