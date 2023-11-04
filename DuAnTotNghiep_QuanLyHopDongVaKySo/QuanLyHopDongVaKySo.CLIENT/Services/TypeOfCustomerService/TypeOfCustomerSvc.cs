using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Services.TypeOfCustomerService
{
    public class TypeOfCustomerSvc : ITypeOfCustomerSvc
    {
        private readonly HttpClient _httpClient;
        public TypeOfCustomerSvc(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<int> AddNew(TypeOfCustomer typeOfCustomer)
        {
            throw new NotImplementedException();
        }

        public Task<List<TypeOfCustomer>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<TypeOfCustomer> GetById(int typeOfCustomer_ID)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(TypeOfCustomer typeOfCustomer)
        {
            throw new NotImplementedException();
        }
    }
}
