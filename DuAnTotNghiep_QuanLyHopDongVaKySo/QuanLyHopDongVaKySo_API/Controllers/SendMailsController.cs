using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMailsController : ControllerBase
    {
        private readonly ISendMailHelper _sendMailHelper;

        public SendMailsController(ISendMailHelper sendMailHelper)
        {
            _sendMailHelper = sendMailHelper;
        }

        [HttpPost("Send")]
        public async Task<ActionResult<string>> SendMail([FromForm] SendMail mail)
        {
            string isSuccess = await _sendMailHelper.SendMail(mail);

            if(isSuccess == null)
            {
                return BadRequest(isSuccess);
            }
            else{
                return Ok(isSuccess);
            }

        }
    }
}
