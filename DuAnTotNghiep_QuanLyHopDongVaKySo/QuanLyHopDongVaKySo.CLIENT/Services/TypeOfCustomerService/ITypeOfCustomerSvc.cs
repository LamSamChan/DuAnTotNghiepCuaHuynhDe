using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Services.TypeOfCustomerService
{
    public interface ITypeOfCustomerSvc
    {
        Task<List<TypeOfCustomer>> GetAll();
        Task<TypeOfCustomer> GetById(int typeOfCustomer_ID);
        Task<int> AddNew(TypeOfCustomer typeOfCustomer);
        Task<int> Update(TypeOfCustomer typeOfCustomer);
    }
}
