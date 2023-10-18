using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.TypeOfCustomerService
{
    public class TypeOfCustomerSvc : ITypeOfCustomerSvc
    {
        private readonly ProjectDbContext _context;

        public TypeOfCustomerSvc(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddNew(TypeOfCustomer typeOfCustomer)
        {
            int isSuccess = 0;
            _context.TypeOfCustomers.Add(typeOfCustomer);
            await _context.SaveChangesAsync();
            isSuccess = typeOfCustomer.TOC_ID;
            return isSuccess;
        }

        public async Task<List<TypeOfCustomer>> GetAll()
        {
            try
            {
                return await _context.TypeOfCustomers.ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<TypeOfCustomer>();
            }
        }

        public async Task<List<TypeOfCustomer>> GetAllNotHidden()
        {
            try
            {
                return await _context.TypeOfCustomers.Where(c => !c.isHidden).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<TypeOfCustomer>();
            }
        }

        public async Task<TypeOfCustomer> GetById(int typeOfCustomer_ID)
        {
            try
            {
                return _context.TypeOfCustomers.FirstOrDefault(c => c.TOC_ID == typeOfCustomer_ID);
            }
            catch (Exception ex)
            {
                return new TypeOfCustomer();
            }
        }

        public async Task<int> Update(TypeOfCustomer typeOfCustomer)
        {
            int status = 0;
            try
            {
                // Lấy đối tượng từ database dựa vào id
                var existingTOC = _context.TypeOfCustomers.FirstOrDefault(c => c.TOC_ID == typeOfCustomer.TOC_ID);

                // Kiểm tra xem đối tượng có tồn tại trong database không
                if (existingTOC == null)
                {
                    return 0;
                }

                // Cập nhật thông tin của đối tượng 
                existingTOC.TOC_ID = typeOfCustomer.TOC_ID;
                existingTOC.TypeName = typeOfCustomer.TypeName;
                existingTOC.isHidden = typeOfCustomer.isHidden;

                // Lưu thay đổi vào database
                await _context.SaveChangesAsync();
                status = typeOfCustomer.TOC_ID;
            }
            catch (System.Exception ex)
            {
                status = 0;
            }
            return status;
        }
    }
}
