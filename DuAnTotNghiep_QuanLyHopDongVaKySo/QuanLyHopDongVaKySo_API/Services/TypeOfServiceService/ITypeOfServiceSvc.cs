using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.TypeOfServiceService
{
    public interface ITypeOfServiceSvc
    {
        Task<List<TypeOfService>> GetAll();
        Task<List<TypeOfService>> GetAllNotHidden();
        Task<TypeOfService> GetById(int? typeOfService_ID);
        Task<int> AddNew(TypeOfService typeOfService);
        Task<int> Update(TypeOfService typeOfService);
    }
}
