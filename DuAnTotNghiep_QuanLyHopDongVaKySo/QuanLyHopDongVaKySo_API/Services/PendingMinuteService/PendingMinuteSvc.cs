using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.DoneContractService;
using QuanLyHopDongVaKySo_API.Services.EmployeeService;
using QuanLyHopDongVaKySo_API.Services.TypeOfServiceService;
using QuanLyHopDongVaKySo_API.Services.InstallationDeviceService;
using System.Linq;
using QuanLyHopDongVaKySo_API.Services.PositionService;

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
        private readonly IPositionSvc _positionSvc;
        public PendingMinuteSvc(ProjectDbContext context, ICustomerSvc customerSvc, IDoneContractSvc doneContractSvc, IEmployeeSvc employeeSvc,
                ITypeOfServiceSvc typeOfServiceSvc, IInstallationDeviceSvc installationDeviceSvc,IPositionSvc positionSvc)
        {
            _context = context;
            _customerSvc = customerSvc;
            _doneContractSvc = doneContractSvc;
            _employeeSvc = employeeSvc;
            _typeOfServiceSvc = typeOfServiceSvc;
            _installationDeviceSvc = installationDeviceSvc;
            _positionSvc = positionSvc;
        }

        public async Task<int> addAsnyc(PendingMinute pMinute)
        {
            try{
                await _context.PendingMinutes.AddAsync(pMinute);
                await _context.SaveChangesAsync();
                return pMinute.PendingMinuteId;

            }catch
            {
                return 0;
            }
            return 0;
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

        public async Task<string> updateAsnyc(PutPMinute pMinute)
        {
            string ret = null;
            var update = await GetById(pMinute.PendingMinuteId);
            try
            {
                if (update != null)
                {
                    update.PendingMinuteId = pMinute.PendingMinuteId;
                    update.DateCreated = pMinute.DateCreated;
                    update.MinuteName = pMinute.MinuteName;
                    update.IsIntallation = pMinute.IsIntallation;
                    update.IsCustomer = pMinute.IsCustomer;
                    update.EmployeeId = pMinute.EmployeeId;
                    update.DoneContractId = pMinute.DoneContractId;
                    update.MinuteFile = pMinute.MinuteFile;
                    update.Base64File = pMinute.Base64File;
                }
                _context.PendingMinutes.Update(update);
                await _context.SaveChangesAsync();
                return update.PendingMinuteId.ToString();
            }
            catch
            {
                return ret;
            }
        }
        public async Task<MinuteInfo> ExportMinute(PendingMinute pMinute, string empId)
        {
            var dContract = await _doneContractSvc.getByIdAsnyc(pMinute.DoneContractId.ToString());
            var cus = await _customerSvc.GetByIdAsync(dContract.CustomerId.ToString());
            var emp = await _employeeSvc.GetById(empId);
            var tOS = await _typeOfServiceSvc.GetById(dContract.TOS_ID);
            var device = await _installationDeviceSvc.GetAllByServiceId(tOS.TOS_ID);
            var EmpPostion = _positionSvc.GetById(emp.PositionID).Result.PositionName;
            MinuteInfo minuteInfo = new MinuteInfo();
            if (cus != null && emp != null)
            {
                minuteInfo.MinuteCreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
                minuteInfo.MinuteContent = "Biên bản lắp đặt dịch vụ " + tOS.ServiceName;
                minuteInfo.MinuteId = "BB"+ pMinute.PendingMinuteId.ToString();
                minuteInfo.ContractId ="HĐ"+ dContract.DContractID.ToString();
                minuteInfo.InstallationCustomer = cus.BuisinessName == null ? cus.FullName : cus.BuisinessName;
                minuteInfo.CustomerName = cus.FullName;
                minuteInfo.CustomerPosition = cus.Position == null ? " " : cus.Position.ToString();
                minuteInfo.CustomerPhone = cus.PhoneNumber;
                minuteInfo.InstallationCompany = "Tech Seal";
                minuteInfo.InstallationPerson = emp.FullName;
                minuteInfo.InstallationPosition = EmpPostion;
                minuteInfo.InstallationPhone = emp.PhoneNumber;
                minuteInfo.InstallationAddress = pMinute.DoneContract.InstallationAddress;
                minuteInfo.InstallationDate = DateTime.Now.ToString("dd/MM/yyyy");
                int count = 0;
                for (int i = 0; i < device.Count; i++)
                {
                    count ++;
                    if (count == 1)
                    {
                        minuteInfo.Number1 = 1;
                        minuteInfo.FirstDevice = device[i].DeviceName;
                        minuteInfo.DeviceStatus1 = device[i].DeviceStatus;
                        minuteInfo.Quantity1 = device[i].DeviceQuantity;
                        continue;
                    }

                    if (count == 2)
                    {
                        minuteInfo.Number2 = 2;
                        minuteInfo.SecondDevice = device[i].DeviceName;
                        minuteInfo.DeviceStatus2 = device[i].DeviceStatus;
                        minuteInfo.Quantity2 = device[i].DeviceQuantity;
                        continue;
                    }

                    if (count == 3)
                    {
                        minuteInfo.Number3 = 3;
                        minuteInfo.ThirdDevice = device[i].DeviceName;
                        minuteInfo.DeviceStatus3 = device[i].DeviceStatus;
                        minuteInfo.Quantity3 = device[i].DeviceQuantity;
                        continue;
                    }

                    if (count == 4)
                    {
                        minuteInfo.Number4 = 4;
                        minuteInfo.FourthDevice = device[i].DeviceName;
                        minuteInfo.DeviceStatus4 = device[i].DeviceStatus;
                        minuteInfo.Quantity4 = device[i].DeviceQuantity;
                        continue;
                    }
                }
            }
            return minuteInfo;
        }

        public async Task<int> updatePMinuteFile(int id, string File, string base64String)
        {
            var update = await GetById(id);
            update.Base64File = base64String;
            update.MinuteFile = File;
            _context.PendingMinutes.Update(update);
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<PendingMinute> getByIdForWinformAsnyc(int id, string cusId)
        {
            var pMinute = _context.PendingMinutes.FirstOrDefault(p => p.PendingMinuteId == id);
            var dContract = _context.DoneContracts.FirstOrDefault(d => d.DContractID == pMinute.DoneContractId);
            if (cusId == dContract.CustomerId.ToString())
            {
                return pMinute;
            }
            return null;
        }
    }
}
