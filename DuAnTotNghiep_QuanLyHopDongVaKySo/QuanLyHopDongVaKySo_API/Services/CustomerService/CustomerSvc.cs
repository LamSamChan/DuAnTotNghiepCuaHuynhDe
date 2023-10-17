using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Helpers;
using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Services.PFXCertificateService;
using Org.BouncyCastle.Ocsp;

namespace QuanLyHopDongVaKySo_API.Services.CustomerService
{
    public class CustomerSvc : ICustomerSvc
    {
        private readonly ProjectDbContext _context;
        private IUploadImageHelper _imageHelpers;
        private IEncodeHelper _encodeHelper;
        private IPFXCertificateSvc _pfxCertificate;

        public CustomerSvc(ProjectDbContext context, IUploadImageHelper imageHelpers, IEncodeHelper encodeHelper, IPFXCertificateSvc pFXCertificate)
        {
            _context = context;
            _imageHelpers = _imageHelpers;
            _encodeHelper = encodeHelper;
            _pfxCertificate = pFXCertificate;
        }
        public async Task<string> AddNewAsync(Customer customer)
        {
            string isSuccess = null;
            try
            {
                string passwordPfx = _encodeHelper.Encode(customer.PhoneNumber);
                string serialPFX = await _pfxCertificate.CreatePFXCertificate("TechSeal", customer.FullName, passwordPfx, false);
                customer.SerialPFX = serialPFX;
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                isSuccess = customer.CustomerId.ToString();
            }
            catch
            {
                return isSuccess;
            }
            return isSuccess;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            try
            {
                return await _context.Customers.ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<Customer>();
            }
        }

        public async Task<Customer> GetByIdAsync(string cusID)
        {
            try
            {
                return _context.Customers.FirstOrDefault(e => e.CustomerId == Guid.Parse(cusID));
            }
            catch (Exception ex)
            {
                return new Customer();
            }
        }


        public async Task<string> UpdateAsync(Customer customer)
        {
            string status = null;
            try
            {
                // Lấy đối tượng từ database dựa vào id
                var existingCus = _context.Customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);

                // Kiểm tra xem đối tượng có tồn tại trong database không
                if (existingCus == null)
                {
                    return status;
                }

                existingCus.CustomerId = customer.CustomerId;
                existingCus.BuisinessName = customer.BuisinessName;
                existingCus.FullName = customer.FullName;
                existingCus.Position = customer.Position;
                existingCus.DateOfBirth = customer.DateOfBirth;
                existingCus.Gender = customer.Gender;
                existingCus.PhoneNumber = customer.PhoneNumber;
                existingCus.Email = customer.Email;
                existingCus.Identification = customer.Identification;
                existingCus.IssuedDate = customer.IssuedDate;
                existingCus.IssuedPlace = customer.IssuedPlace;
                existingCus.Nationality = customer.Nationality;
                existingCus.BankAccount = customer.BankAccount;
                existingCus.BankName = customer.BankName;
                existingCus.TaxIDNumber = customer.TaxIDNumber;
                existingCus.BillingAddress = customer.BillingAddress;
                existingCus.IsLocked = customer.IsLocked;
                existingCus.Note = customer.Note;
                existingCus.SerialPFX = customer.SerialPFX;
                existingCus.TOC_ID = customer.TOC_ID;
                
                await _context.SaveChangesAsync();
                status = customer.CustomerId.ToString();
            }
            catch (Exception ex)
            {
                return status;
            }
            return status;
        } 
    }
}
