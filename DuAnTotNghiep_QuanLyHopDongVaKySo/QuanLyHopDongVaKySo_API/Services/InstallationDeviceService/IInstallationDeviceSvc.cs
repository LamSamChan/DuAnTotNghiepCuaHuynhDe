using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.InstallationDeviceService
{
    public interface IInstallationDeviceSvc
    {
        Task<List<InstallationDevice>> GetAll();
        Task<List<InstallationDevice>> GetAllByServiceId(int? serviceID);
        Task<InstallationDevice> GetById(int deviceID);
        Task<int> AddNew(InstallationDevice device);
        Task<int> Update(InstallationDevice device);
        Task<int> Delete(int deviceID);
    }
}
