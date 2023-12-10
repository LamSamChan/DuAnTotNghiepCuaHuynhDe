
using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
namespace QuanLyHopDongVaKySo.CLIENT.Services.TOSServices
{
    public interface ITOSService
    {
        Task<List<TypeOfService>> GetAll();
        Task<TypeOfService> GetById(int typeOfService_ID);
        Task<PutTOS> GetByPutId(int typeOfService_ID);
        Task<int> AddNew(PostTOS typeOfService);
        Task<int> Update(PutTOS typeOfService);
    }
}
