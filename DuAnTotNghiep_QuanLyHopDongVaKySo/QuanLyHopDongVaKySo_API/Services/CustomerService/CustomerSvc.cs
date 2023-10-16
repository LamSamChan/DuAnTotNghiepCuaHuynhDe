using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Helpers;
using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Services.PFXCertificateService;
namespace QuanLyHopDongVaKySo_API.Services.CustomerService
{
    public class CustomerSvc : ICustomerSvc
    {
        private readonly ProjectDbContext _context;
        private UploadImageHelper _imageHelpers;
        private EncodeHelper _enocdeHelper;
        private IPFXCertificateSvc _pFXCertificate;

        public CustomerSvc(ProjectDbContext context, UploadImageHelper imageHelpers, EncodeHelper encodeHelper,IPFXCertificateSvc pFXCertificate)
        {
            _context = context;
            _imageHelpers = _imageHelpers;
            _enocdeHelper = encodeHelper;
            _pFXCertificate = pFXCertificate;
        }
        public async Task<string> addCustomerAsnyc(PostCustomer customer)
        {
            string ret = null;
            try{
                string serialPFX = await _pFXCertificate.CreatePFXCertificate("TechSeal", customer.FullName, _enocdeHelper.Encode(customer.Identification), true);
                Customer add = new Customer
                {
                    CustomerId = Guid.NewGuid(),
                    BuisinessName = customer.BuisinessName,
                    FullName = customer.FullName,
                    Position = customer.Position,
                    DateOfBirth = customer.DateOfBirth,
                    Gender = customer.Gender,
                    PhoneNumber = customer.PhoneNumber,
                    Email = customer.Email,
                    Identification = customer.Identification,
                    IssuedDate = customer.IssuedDate,
                    IssuedPlace = customer.IssuedPlace,
                    Nationality = customer.Nationality,
                    BankAccount = customer.BankAccount,
                    BankName = customer.BankName,
                    TaxIDNumber = customer.TaxIDNumber,
                    BillingAddress = customer.BillingAddress,
                    IsLocked = false,
                    Note = customer.Note,
                    SerialPFX = serialPFX,
                    TOC_ID = customer.TOC_ID
                };
                await _context.Customers.AddAsync(add);
                await _context.SaveChangesAsync();
                ret = add.CustomerId.ToString();
            }
            catch
            {
                
            }
            return ret;
        }

        public async Task<bool> deleteCustomerAsnyc(Guid id)
        {
            var delete = await getCustomerAsnyc(id);
            if(delete != null)
            {
                _context.Customers.Remove(delete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Customer> getCustomerAsnyc(Guid id)
        {
            return await _context.Customers.Where(c => c.CustomerId == id).FirstOrDefaultAsync();
        }

        public async Task<List<Customer>> getCustomersAsnyc()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<string> updateCustomerAsnyc(PutCustomer customer)
        {
            string ret = null;
            try{
                var update = await getCustomerAsnyc(customer.CustomerId);
                if(update != null)
                {
                    Customer add = new Customer
                    {
                        CustomerId = Guid.NewGuid(),
                        BuisinessName = customer.BuisinessName,
                        FullName = customer.FullName,
                        Position = customer.Position,
                        DateOfBirth = customer.DateOfBirth,
                        Gender = customer.Gender,
                        PhoneNumber = customer.PhoneNumber,
                        Email = customer.Email,
                        Identification = customer.Identification,
                        IssuedDate = customer.IssuedDate,
                        IssuedPlace = customer.IssuedPlace,
                        Nationality = customer.Nationality,
                        BankAccount = customer.BankAccount,
                        BankName = customer.BankName,
                        TaxIDNumber = customer.TaxIDNumber,
                        BillingAddress = customer.BillingAddress,
                        IsLocked = customer.IsLocked,
                        Note = customer.Note,
                        SerialPFX = customer.SerialPFX,
                        TOC_ID = customer.TOC_ID
                    };
                    await _context.Customers.AddAsync(add);
                    await _context.SaveChangesAsync();
                    ret = add.CustomerId.ToString();
                }
                
            }
            catch
            {
                
            }
            return ret;
        }
    }
}
