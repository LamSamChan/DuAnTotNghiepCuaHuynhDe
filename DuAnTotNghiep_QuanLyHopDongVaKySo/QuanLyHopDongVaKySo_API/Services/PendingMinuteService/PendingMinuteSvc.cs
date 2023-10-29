using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.DoneContractService;
using QuanLyHopDongVaKySo_API.Services.EmployeeService;
using QuanLyHopDongVaKySo_API.Services.TypeOfServiceService;
using QuanLyHopDongVaKySo_API.Services.InstallationDeviceService;
namespace QuanLyHopDongVaKySo_API.Services.PendingMinuteService

{
    public class PendingMinuteSvc : IPendingMinuteSvc
    {
        private readonly ProjectDbContext _context;
        private readonly ICustomerSvc _customerSvc;
        private readonly IDoneContractSvc _doneContractSvc;
        private readonly IEmployeeSvc _employeeSvc;
        private readonly ITypeOfServiceSvc _typeOfServiceSvc;
        private readonly IInstallationDeviceSvc _installationDeviceSvc;
        public PendingMinuteSvc(ProjectDbContext context, ICustomerSvc customerSvc, IDoneContractSvc doneContractSvc, IEmployeeSvc employeeSvc,
                ITypeOfServiceSvc typeOfServiceSvc, IInstallationDeviceSvc installationDeviceSvc)
        {
            _context = context;
            _customerSvc = customerSvc;
            _doneContractSvc = doneContractSvc;
            _employeeSvc = employeeSvc;
            _typeOfServiceSvc = typeOfServiceSvc;
            _installationDeviceSvc = installationDeviceSvc;
        }

        public async Task<string> addAsnyc(PostPMinute pMinute)
        {
            string ret = null;
            try{
                PendingMinute add = new PendingMinute()
                {
                    DateCreated = DateTime.Now,
                    MinuteName = pMinute.MinuteName,
                    IsIntallation = false,
                    IsCustomer = false,
                    MinuteFile = "",
                    EmployeeId = pMinute.EmployeeId,
                    DoneContractId = pMinute.DoneContractId,
                    TMinuteId = pMinute.TMinuteId,
                };
                await _context.PendingMinutes.AddAsync(add);
                await _context.SaveChangesAsync();
                return add.PendingMinuteId.ToString();
            }catch
            {
                return ret;
            }
            return ret;
        }

        public async Task<int> DeletePMinute(int pMinuteId)
        {
            int status = 0;
            try
            {
                var pMinute = _context.PendingMinutes.FirstOrDefault(m => m.PendingMinuteId == pMinuteId);
                _context.Remove(pMinute);
                await _context.SaveChangesAsync();
                status = pMinute.PendingMinuteId;
            }
            catch (Exception ex)
            {
                return status;
            }
            return status;
        }

        public async Task<List<PendingMinute>> GetAll()
        {
            try
            {
                return await _context.PendingMinutes.ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<PendingMinute>();
            }
        }

        public async Task<PendingMinute> GetById(int pMinuteId)
        {
            try
            {
                return _context.PendingMinutes.FirstOrDefault(p => p.PendingMinuteId == pMinuteId);
            }
            catch (Exception ex)
            {
                return new PendingMinute();
            }
        }

        public async Task<int> GetJobFormIRequirement(PendingMinute pendingMinute)
        {
            try
            {
                int isSuccess = 0;
                _context.PendingMinutes.Add(pendingMinute);
                await _context.SaveChangesAsync();
                isSuccess = pendingMinute.PendingMinuteId;
                return isSuccess;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<PendingMinute>> GetListByEmpId(string EmployeeId)
        {
            try
            {
                return await _context.PendingMinutes.Where(p => p.EmployeeId == Guid.Parse(EmployeeId)).ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<PendingMinute>();
            }
        } 
        public async Task<MinuteInfo> ExportContract(PendingMinute pMinute, string empId)
        {
            var dContract = await _doneContractSvc.getByIdAsnyc(pMinute.DoneContractId);
            var cus = await _customerSvc.GetByIdAsync(dContract.CustomerId.ToString());
            var emp = await _employeeSvc.GetById(empId);
            var tOS = await _typeOfServiceSvc.GetById(dContract.TOS_ID);
            var device = await _installationDeviceSvc.GetAllByServiceId(tOS.TOS_ID);
            var deviceNames = device.Select(d => d == null ? " " : d.DeviceName).ToList();

            MinuteInfo minuteInfo = new MinuteInfo();
            if (cus != null && emp != null)
            {
                minuteInfo.MinuteContent = "Bien ban lap dat dich vu " + tOS.ServiceName + "hop dong" + dContract.DConTractName;
                minuteInfo.MinuteId = pMinute.PendingMinuteId.ToString();
                minuteInfo.ContractId = dContract.DContractID.ToString();
                minuteInfo.InstallationCompany = "Tech steal";
                minuteInfo.InstallationCustomer = emp.FullName;
                minuteInfo.InstallationPosition = emp.Position == null? " " : emp.Position.ToString();
                minuteInfo.InstallationPosition = emp.PhoneNumber;
                minuteInfo.InstallationAddress = cus.Address;
                minuteInfo.CustomerName = cus.FullName;
                minuteInfo.CustomerPosition = cus.Position == null? " " : cus.Position.ToString();
                minuteInfo.CustomerPhone = cus.PhoneNumber;
                minuteInfo.InstallationDate = DateTime.Now.AddDays(2);
                minuteInfo.Number1 = 1;
                minuteInfo.FirstDevice = deviceNames[0];
                minuteInfo.Quantity1 = 1;
                minuteInfo.Number1 = 2;
                minuteInfo.SecondDevice = " ";
                minuteInfo.Quantity1 = 1;
                minuteInfo.Number1 = 3;
                minuteInfo.ThirdDevice = " ";
                minuteInfo.Quantity1 = 1;
                minuteInfo.Number1 = 4;
                minuteInfo.FourthDevice = " ";
                minuteInfo.Quantity1 = 1;

            }
            return minuteInfo;
        }

        public async Task<int> updatePMinuteFile(int id, string File)
        {
            var update = await GetById(id);

            update.MinuteFile = File;
            _context.PendingMinutes.Update(update);
            await _context.SaveChangesAsync();
            return 1;
        }
    }
}
