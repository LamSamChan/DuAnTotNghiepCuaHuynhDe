using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.InstallationDeviceService
{
    public class InstallationDeviceSvc:IInstallationDeviceSvc
    {
        private readonly ProjectDbContext _context;

        public InstallationDeviceSvc(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddNew(InstallationDevice device)
        {
            int isSuccess = 0;
            _context.InstallationDevices.Add(device);
            await _context.SaveChangesAsync();
            isSuccess = device.Device_ID;
            return isSuccess;
        }

        public async Task<int> Delete(int deviceID)
        {
            int isSuccess = 0;
            var device = _context.InstallationDevices.FirstOrDefault(d => d.Device_ID == deviceID);
            if (device != null)
            {
                _context.InstallationDevices.Remove(device);
                await _context.SaveChangesAsync();
                isSuccess = device.Device_ID;
                return isSuccess;
            }
            return isSuccess;
        }

        public async Task<List<InstallationDevice>> GetAll()
        {
            try
            {
                return await _context.InstallationDevices.ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<InstallationDevice>();
            }
        }

        public async Task<List<InstallationDevice>> GetAllByServiceId(int? serviceID)
        {
            try
            {
                return await _context.InstallationDevices.Where(d => d.TOS_ID == serviceID).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<InstallationDevice>();
            }
        }

        public async Task<InstallationDevice> GetById(int deviceID)
        {
            try
            {
                return _context.InstallationDevices.FirstOrDefault(d => d.Device_ID == deviceID);
            }
            catch (Exception ex)
            {
                return new InstallationDevice();
            }
        }

        public async Task<int> Update(InstallationDevice device)
        {
            int status = 0;
            try
            {
                // Lấy đối tượng từ database dựa vào id
                var existingDevice = _context.InstallationDevices.FirstOrDefault(d => d.Device_ID == device.Device_ID);

                // Kiểm tra xem đối tượng có tồn tại trong database không
                if (existingDevice == null)
                {
                    return 0;
                }

                // Cập nhật thông tin của đối tượng 
                existingDevice.Device_ID = device.Device_ID;
                existingDevice.DeviceName = device.DeviceName;
                existingDevice.DeviceStatus = device.DeviceStatus;
                existingDevice.DeviceQuantity = device.DeviceQuantity;
                existingDevice.TOS_ID = device.TOS_ID;

                // Lưu thay đổi vào database
                await _context.SaveChangesAsync();
                status = device.Device_ID;
            }
            catch (System.Exception ex)
            {
                status = 0;
            }
            return status;
        }
    }
}
