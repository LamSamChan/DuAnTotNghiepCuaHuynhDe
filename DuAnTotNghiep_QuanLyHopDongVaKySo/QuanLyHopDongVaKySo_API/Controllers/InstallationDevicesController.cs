using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.InstallationDeviceService;
using QuanLyHopDongVaKySo_API.Services.TypeOfServiceService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class InstallationDevicesController : ControllerBase
    {
        private readonly IInstallationDeviceSvc _installationDeviceSvc;
        private readonly ITypeOfServiceSvc _typeOfServiceSvc;

        public InstallationDevicesController(IInstallationDeviceSvc installationDeviceSvc, ITypeOfServiceSvc typeOfServiceSvc)
        {
            _installationDeviceSvc = installationDeviceSvc;
            _typeOfServiceSvc = typeOfServiceSvc;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstallationDevice>>> GetAll()
        {
            return Ok(await _installationDeviceSvc.GetAll());
        }

        [HttpGet("GetAllByServiceID/{serviceID}")]
        public async Task<ActionResult<IEnumerable<InstallationDevice>>> GetAllByServiceID(int serviceID)
        {
            return Ok(await _installationDeviceSvc.GetAllByServiceId(serviceID));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InstallationDevice>> GetById(int id)
        {
            InstallationDevice device = await _installationDeviceSvc.GetById(id);
            if (device != null)
            {
                return Ok(device);
            }
            else
            {
                return BadRequest(device);
            }
        }

        [HttpPost("AddNew")]
        public async Task<ActionResult<int>> AddNew(InstallationDevice device)
        {
            var getListBySvcId = await _installationDeviceSvc.GetAllByServiceId(device.TOS_ID);
            if (getListBySvcId != null)
            {
                if (getListBySvcId.Count == 4)
                {
                    return BadRequest("Dịch vụ hiện tại đã đủ thiết bị lắp đặt, hãy cập nhật hoặc xoá thiết bị để có thể thêm thiết bị mới !");
                }
            }
            var tos = await _typeOfServiceSvc.GetById(device.TOS_ID);
            if (tos != null)
            {
                int isError = await _installationDeviceSvc.AddNew(device);
                if (isError != 0)
                {
                    return Ok(isError);
                }
                else { return BadRequest(isError); }
            }
            else
            {
                return BadRequest("Dịch vụ hiện tại không tồn tại !");
            }
           
        }

        [HttpPut("Update")]
        public async Task<ActionResult<int>> Update(InstallationDevice device)
        {
            int isError = await _installationDeviceSvc.Update(device);
            if (isError != 0)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }

        [HttpDelete("Delete/{deviceID}")]
        public async Task<ActionResult<int>> Delete(int deviceID)
        {
            int isError = await _installationDeviceSvc.Delete(deviceID);
            if (isError != 0)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }
    }
}
