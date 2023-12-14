using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
using QuanLyHopDongVaKySo_API.Services;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.DoneContractService;
using QuanLyHopDongVaKySo_API.Services.DoneMinuteService;
using QuanLyHopDongVaKySo_API.Services.InstallationRequirementService;
using QuanLyHopDongVaKySo_API.Services.PendingMinuteService;
using QuanLyHopDongVaKySo_API.Services.TemplateMinuteService;
using QuanLyHopDongVaKySo_API.Services.TypeOfServiceService;
using System.Reflection;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class DMinuteController : ControllerBase
    {
        private readonly IDoneMinuteSvc _doneMinuteSvc;
        private readonly IPendingMinuteSvc _pendingMinuteSvc;
        private readonly IUploadFileHelper _uploadFileHelper;
        private readonly IDoneContractSvc _doneContractSvc;
        private readonly ICustomerSvc _customerSvc;
        private readonly ISendMailHelper _sendMailHelper;


        public DMinuteController(IDoneMinuteSvc doneMinuteSvc, IPendingMinuteSvc pendingMinuteSvc, IUploadFileHelper uploadFileHelper,
            IDoneContractSvc doneContractSvc, ICustomerSvc customerSvc, ISendMailHelper sendMailHelper)
        {
            _doneMinuteSvc = doneMinuteSvc;
            _pendingMinuteSvc = pendingMinuteSvc;
            _uploadFileHelper = uploadFileHelper;
            _doneContractSvc = doneContractSvc;
            _customerSvc = customerSvc;
            _sendMailHelper = sendMailHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        { 
            return Ok(await _doneMinuteSvc.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _doneMinuteSvc.GetById(int.Parse(id)));
        }

        [HttpGet("Employee/{id}")]
        public async Task<IActionResult> GetByEmployeeId(string id)
        {
            return Ok(await _doneMinuteSvc.GetListByEmpId(id));
        }

        [HttpPost("SignMinuteWithUSBToken")]
        public async Task<IActionResult> SignByUSBToken([FromBody] PostDMinute postDMinute)
        {
            if (postDMinute.Base64File == null)
            {
                return BadRequest();
            }
            var pMinute = await _pendingMinuteSvc.GetById(postDMinute.PMinuteID);

            if (pMinute == null)
            {
                return BadRequest();
            }
            else
            {
                var dContract = await _doneContractSvc.getByIdAsnyc(pMinute.DoneContractId.ToString());
                DoneMinute doneMinute = new DoneMinute()
                {
                    DateDone = DateTime.Now,
                    MinuteName = postDMinute.DMinuteName,
                    Base64File = postDMinute.Base64File,
                    EmployeeId = pMinute.EmployeeId,
                    MinuteFile ="",
                };
                var result = await _doneMinuteSvc.AddDMinuteFromSignByUSBToken(doneMinute);
                string minutePath = null;

                if (result != null)
                {
                    IFormFile file = _uploadFileHelper.ConvertBase64ToIFormFile(postDMinute.Base64File, "bb"+result.DoneMinuteID.ToString()+"_SignedByUsbToken", "application/pdf");
                    minutePath = _uploadFileHelper.UploadFile(file, $"AppData/DContracts", dContract.DContractID.ToString(), ".pdf");
                    result.MinuteFile = minutePath;

                    var updateResult = await _doneMinuteSvc.UpdateMinuteFromSignByUSBToken(result);
                    if (updateResult != null)
                    {
                        
                        PutDContract putDContract = new PutDContract()
                        {
                            DContractID = dContract.DContractID.ToString(),
                            IsInEffect = true,
                            DoneMinuteId = result.DoneMinuteID,
                        };

                        var updatedContract = await _doneContractSvc.updateAsnycDMinute(putDContract);

                        if (updatedContract == null)
                        {
                            return BadRequest();
                        }

                        FileStream fs = new FileStream(pMinute.MinuteFile, FileMode.Open, FileAccess.Read);
                        fs.Close();
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                        System.IO.File.Delete(pMinute.MinuteFile);


                        await _pendingMinuteSvc.DeletePMinute(pMinute.PendingMinuteId);

                        var customer = await _customerSvc.GetByIdAsync(dContract.CustomerId.ToString());


                        Task.Run(() => SendMailToCustomerWithFile(System.IO.File.ReadAllBytes(dContract.DContractFile), System.IO.File.ReadAllBytes(result.MinuteFile), customer));

                        return Ok(result.DoneMinuteID + "*" + pMinute.PendingMinuteId);
                    }
                    return BadRequest();
                }
                return BadRequest();
            }

        }

        private async Task<string> SendMailToCustomerWithFile(byte[] bytesContract, byte[] bytesMinute, Customer customer)
        {

            string content = System.IO.File.ReadAllText("AppData\\TemplateSendMail\\camon.html").Replace("[TENKHACHHANG]", customer.FullName);


            SendMail mail = new SendMail();
            mail.Subject = "Hợp đồng Từ TechSeal";
            mail.ReceiverName = customer.FullName;
            mail.ToMail = customer.Email;
            mail.HtmlContent = content;
            string isSuccess = await _sendMailHelper.SendMailWithFile(mail, bytesContract, bytesMinute);
            if (isSuccess != null)
            {
                return "Đã gửi thành công";
            }
            else
            {
                return null;
            }
        }
    }
}