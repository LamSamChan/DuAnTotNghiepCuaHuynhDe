using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.PFXCertificateService;
using QuanLyHopDongVaKySo_API.Services.PositionService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PFXCertificatesController : ControllerBase
    {
        private readonly IPFXCertificateSvc _pfxCertificate;
        public PFXCertificatesController(IPFXCertificateSvc pfxCertificate)
        {
            _pfxCertificate = pfxCertificate;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PFXCertificate>>> GetAll()
        {
            return Ok(await _pfxCertificate.GetAll());
        }

        [HttpGet("AboutToExpire")]
        public async Task<ActionResult<IEnumerable<PFXCertificate>>> GetAllAboutToExpire()
        {
            return Ok(await _pfxCertificate.GetAllAboutToExpire());
        }
         
        [HttpGet("Expire")]
        public async Task<ActionResult<IEnumerable<PFXCertificate>>> GetAllExpire()
        {
            return Ok(await _pfxCertificate.GetAllExpire());
        }

        [HttpGet("{serial}")]
        public async Task<ActionResult<PFXCertificate>> GetById(string serial)
        {
            PFXCertificate certificate = await _pfxCertificate.GetById(serial);
            if (certificate != null)
            {
                return Ok(certificate);
            }
            else
            {
                return BadRequest(certificate);
            }
        }

        [HttpPost("UpdateNotAfter")]
        public async Task<ActionResult<string>> UpdateNotAfter(string pfxFilePath, string password, bool isEmployee)
        {
            string isUpdateToDatabase = null;
            PFXCertificate isUpdateNotAfter = await _pfxCertificate.UpdateNotAfter(pfxFilePath,password,isEmployee);

            if (isUpdateNotAfter != null)
            {
                isUpdateToDatabase = await _pfxCertificate.UpdateInfoToDatabase(isUpdateNotAfter);

                if (isUpdateToDatabase != null)
                {
                    return Ok(isUpdateToDatabase);
                }
                else
                {
                    return BadRequest(isUpdateToDatabase);
                }
                
            }
            else { return BadRequest(isUpdateNotAfter); }

        }

    }
}
