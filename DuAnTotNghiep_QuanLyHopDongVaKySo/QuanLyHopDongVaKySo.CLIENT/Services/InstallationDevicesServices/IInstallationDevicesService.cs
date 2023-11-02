using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Services.InstallationDevicesServices
{
    public interface IInstallationDevicesService
    {
        Task<List<InstallationDevice>> GetAllDevice();
        Task<InstallationDevice> GetDeviceById(int id);
        Task<string> AddNewDevice(InstallationDevice postDevice);
        Task<string> UpdateDevice(InstallationDevice putDevice); 
        Task<int> DelectDevice(int id);
        Task<List<InstallationDevice>> GetAllByServiceId(int serviceID);
    }
}

