using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
namespace QuanLyHopDongVaKySo.CLIENT.Services.TMinuteServices
{
    public interface ITMinuteService
    {
        Task<TemplateMinute> GetById(int id);
        Task<List<TemplateMinute>> GetAll();
        Task<int> AddNew(PostTMinute tMinute);
        Task<int> Update(PutTMinute tMinute);
        Task<int> DeleteTMinute(int id);
    }
}
