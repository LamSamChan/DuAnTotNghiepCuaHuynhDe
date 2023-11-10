using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.TypeOfServiceService
{
    public class TypeOfServiceSvc : ITypeOfServiceSvc
    {
        private readonly ProjectDbContext _context;

        public TypeOfServiceSvc(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddNew(TypeOfService typeOfService)
        {
            int isSuccess = 0;
            typeOfService.DateAdded = DateTime.Now;
            typeOfService.DateUpdated = DateTime.Now;
            _context.TypeOfServices.Add(typeOfService);
            await _context.SaveChangesAsync();
            isSuccess = typeOfService.TOS_ID;
            return isSuccess;
        }

        public async Task<List<TypeOfService>> GetAll()
        {
            try
            {
                return await _context.TypeOfServices.ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<TypeOfService>();
            }
        }

        public async Task<List<TypeOfService>> GetAllNotHidden()
        {
            try
            {
                return await _context.TypeOfServices.Where(s => !s.isHidden).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<TypeOfService>();
            }
        }

        public async Task<TypeOfService> GetById(int? typeOfService_ID)
        {
            try
            {
                return _context.TypeOfServices.FirstOrDefault(s => s.TOS_ID == typeOfService_ID);
            }
            catch (Exception ex)
            {
                return new TypeOfService();
            }
        }

        public async Task<int> Update(TypeOfService typeOfService)
        {
            int status = 0;
            try
            {
                // Lấy đối tượng từ database dựa vào id
                var existingTOS = _context.TypeOfServices.FirstOrDefault(s => s.TOS_ID == typeOfService.TOS_ID);

                // Kiểm tra xem đối tượng có tồn tại trong database không
                if (existingTOS == null)
                {
                    return 0;
                }

                // Cập nhật thông tin của đối tượng 
                existingTOS.TOS_ID = typeOfService.TOS_ID;
                existingTOS.ServiceName = typeOfService.ServiceName;
                existingTOS.Price = typeOfService.Price;
                existingTOS.PerTime = typeOfService.PerTime;
                existingTOS.DateAdded = existingTOS.DateAdded;
                existingTOS.DateUpdated = DateTime.Now;
                existingTOS.isHidden = typeOfService.isHidden;
                existingTOS.templateContractID = typeOfService.templateContractID;
                existingTOS.templateMinuteID = typeOfService.templateMinuteID;
                // Lưu thay đổi vào database
                await _context.SaveChangesAsync();
                status = typeOfService.TOS_ID;
            }
            catch (System.Exception ex)
            {
                status = 0;
            }
            return status;
        }
    }
}
