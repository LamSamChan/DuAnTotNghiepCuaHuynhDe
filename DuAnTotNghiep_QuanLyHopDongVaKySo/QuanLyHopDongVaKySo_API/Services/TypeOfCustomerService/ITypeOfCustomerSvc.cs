using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.TypeOfCustomerService
{
    public interface ITypeOfCustomerSvc
    {
        Task<List<TypeOfCustomer>> GetAll();
        Task<List<TypeOfCustomer>> GetAllNotHidden();
        Task<List<TypeOfCustomer>> GetAllHidden();
        Task<TypeOfCustomer> GetById(int typeOfCustomer_ID);
        Task<int> AddNew(TypeOfCustomer typeOfCustomer);
        Task<int> Update(TypeOfCustomer typeOfCustomer);
    }
}
