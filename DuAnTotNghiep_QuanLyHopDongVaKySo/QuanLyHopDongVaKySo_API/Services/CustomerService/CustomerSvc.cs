using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Helpers;
using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Services.PFXCertificateService;
using Org.BouncyCastle.Ocsp;
using iTextSharp.text;

namespace QuanLyHopDongVaKySo_API.Services.CustomerService
{
    public class CustomerSvc : ICustomerSvc
    {
        private readonly ProjectDbContext _context;
        private readonly IUploadFileHelper _imageHelpers;
        private readonly IEncodeHelper _encodeHelper;
        private readonly IPFXCertificateSvc _pfxCertificate;
        public CustomerSvc(ProjectDbContext context, IUploadFileHelper imageHelpers, IEncodeHelper encodeHelper, IPFXCertificateSvc pFXCertificate)
        {
            _context = context;
            _imageHelpers = imageHelpers;
            _encodeHelper = encodeHelper;
            _pfxCertificate = pFXCertificate;
        }
        public async Task<string> AddNewAsync(Customer customer)
        {
            string isSuccess = null;
            string check = await IsFieldExist(customer);
            try
            {
                if (String.Compare(check, "0") != 0)
                {
                    return check;
                }
                else
                {
                    string subjectName = customer.BuisinessName != null ? customer.BuisinessName : customer.FullName;
                    string passwordPfx = _encodeHelper.Encode(customer.PhoneNumber);
                    string serialPFX = await _pfxCertificate.CreatePFXCertificate("TechSeal", subjectName, passwordPfx, false);
                    customer.SerialPFX = serialPFX;
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                    isSuccess = customer.CustomerId.ToString();
                }
            }
            catch (Exception ex)
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

        public async Task<Customer> GetByIdentificationAsync(string identification)
        {
            try
            {
                return _context.Customers.FirstOrDefault(e => e.Identification == identification);
            }
            catch (Exception ex)
            {
                return new Customer();
            }
        }

        public async Task<Customer> GetBySerialPFXAsync(string serial)
        {
            try
            {
                return _context.Customers.FirstOrDefault(e => e.SerialPFX == serial);
            }
            catch (Exception ex)
            {
                return new Customer();
            }
        }

        //return -1: trung mail, -2: trung sđth,-3: trung cccd, -4 trung bank account, -5 trung ma so thue, 0: oke
        public async Task<string> IsFieldExist(Customer customer)
        {
            List<string> toc = new List<string>()
            {
                new string("Cá nhân"),
                new string("Doanh nghiệp")
            };
            string isFoundTOC = "1";

            foreach (var item in toc)
            {
                if (customer.typeofCustomer == item)
                {
                    var customerWithTOC_ID = await _context.Customers.Where(e => e.typeofCustomer == item).ToListAsync();
                    
                    isFoundTOC = "0";
                    string phoneNumber = null;

                    if (customer.PhoneNumber.StartsWith("+84"))
                    {
                        phoneNumber = customer.PhoneNumber.Substring(3);
                    }
                    else
                    {
                        phoneNumber = customer.PhoneNumber.Substring(1);
                    }

                    foreach (var cus in customerWithTOC_ID)
                    {
                        if (cus.CustomerId == customer.CustomerId) continue;
                        string existPhoneNumber = null;

                        if (cus.PhoneNumber.StartsWith("+84"))
                        {
                            existPhoneNumber = cus.PhoneNumber.Substring(3);
                        }
                        else
                        {
                            existPhoneNumber = cus.PhoneNumber.Substring(1);
                        }

                        if (customer.Email == cus.Email)
                        {
                            return "-1";
                        }
                        else if (phoneNumber == existPhoneNumber)
                        {
                            return "-2";
                        }
                        else if (customer.Identification == cus.Identification)
                        {
                            return "-3";
                        }
                        else if (customer.BankAccount == cus.BankAccount)
                        {
                            return "-4";
                        }
                        else if (customer.TaxIDNumber == cus.TaxIDNumber)
                        {
                            if (customer.TaxIDNumber == null && cus.TaxIDNumber == null)
                            {
                                return isFoundTOC;
                            }
                            return "-5";
                        }
                    }
                }
            }
            return isFoundTOC;
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

                existingCus.CustomerId = existingCus.CustomerId;
                existingCus.BuisinessName = customer.BuisinessName;
                existingCus.FullName = customer.FullName;
                existingCus.Position = customer.Position;
                existingCus.DateOfBirth = customer.DateOfBirth;
                existingCus.Gender = customer.Gender;
                existingCus.PhoneNumber = customer.PhoneNumber;
                existingCus.Email = customer.Email;
                existingCus.BuisinessNumber = customer.BuisinessNumber;
                existingCus.BNDate = customer.BNDate;
                existingCus.BNPlace = customer.BNPlace;
                existingCus.PowerOfAttorneyNum = customer.PowerOfAttorneyNum;
                existingCus.DatePOA = customer.DatePOA;
                existingCus.WhoPOA = customer.WhoPOA;
                existingCus.Identification = customer.Identification;
                existingCus.IssuedDate = customer.IssuedDate;
                existingCus.IssuedPlace = customer.IssuedPlace;
                existingCus.Nationality = customer.Nationality;
                existingCus.BankAccount = customer.BankAccount;
                existingCus.BankName = customer.BankName;
                existingCus.TaxIDNumber = customer.TaxIDNumber;
                existingCus.Address = customer.Address;
                existingCus.ChargeNoticeAddress = customer.ChargeNoticeAddress;
                existingCus.BillingAddress = customer.BillingAddress;
                existingCus.IsLocked = customer.IsLocked;
                existingCus.Note = customer.Note;
                existingCus.SerialPFX = existingCus.SerialPFX;
                existingCus.typeofCustomer = customer.typeofCustomer;

                status = await IsFieldExist(existingCus);
                if (String.Compare(status, "0") != 0)
                {
                    return status;
                }
                else
                {
                    await _context.SaveChangesAsync();
                    status = customer.CustomerId.ToString();
                }
            }
            catch (Exception ex)
            {
                return status;
            }
            return status;
        } 
    }
}
