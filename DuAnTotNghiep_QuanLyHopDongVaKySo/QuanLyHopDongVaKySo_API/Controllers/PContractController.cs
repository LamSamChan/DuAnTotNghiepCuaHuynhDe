using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.PendingContractService;
using QuanLyHopDongVaKySo_API.Services.TemplateContractService;
using QuanLyHopDongVaKySo_API.Helpers;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Data.Common;
namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PContractController:ControllerBase
    {
        private readonly IPendingContractSvc _PContractSvc;
        private readonly ICustomerSvc _CustomerSvc;
        private readonly ITemplateContractSvc _TContractSvc;

        public PContractController(IPendingContractSvc PContractSvc, ICustomerSvc CustomerSvc, ITemplateContractSvc TContractSvc)
        {
            _PContractSvc = PContractSvc;
            _CustomerSvc = CustomerSvc;
            _TContractSvc = TContractSvc;
        }

        [HttpPost]
        public async Task<IActionResult> AddPContractAsnyc([FromForm] PostPendingContract PContract)
        {
            var cus = await _CustomerSvc.GetByIdAsync(PContract.CustomerId.ToString());
            var tContract =await _TContractSvc.getTContractAsnyc(PContract.TContractId);
            if(ModelState.IsValid)
            {
                string id_Pcontract = await _PContractSvc.addPContractAsnyc(PContract);
                
                if(cus != null)
                {
                    string pdfFilePath = tContract.TContractFile ;
                    string outputPdfFile = "AppData/PContracts/"+id_Pcontract+".pdf";
                    PdfReader pdfReader = new PdfReader(pdfFilePath);
                    PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(outputPdfFile, FileMode.Create));

                    // Tạo một font cho trường văn bản
                    BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);

                    // Thiết lập font và kích thước cho trường văn bản
                    Font font = new Font(bf, 12);
                    // Tạo một trường văn bản mới và thiết lập nội dung
                    PdfContentByte cb = pdfStamper.GetOverContent(1); // 1 là số trang bạn muốn thêm trường văn bản
                    PdfContentByte cb2 = pdfStamper.GetOverContent(2); // 1 là số trang bạn muốn thêm trường văn bản
                    ColumnText.ShowTextAligned(cb, Element.ALIGN_BASELINE, new Phrase(cus.CustomerId.ToString(), font), 424, 810, 0);
                    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(id_Pcontract, font), 424, 749, 0);
                    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(DateTime.Now.Date.ToString("dd/MM/yyyy"), font), 424, 733, 0);
                    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(cus.FullName, font), 110, 298, 0);
                    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(cus.DateOfBirth.Date.ToString("dd/MM/yyyy"), font), 140, 268, 0);
                    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(cus.Identification, font), 210, 239, 0);
                    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(cus.IssuedDate.Date.ToString("dd/MM/yyyy"), font), 347, 239, 0);
                    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(cus.IssuedPlace, font), 450, 239, 0);
                    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(cus.BankName, font), 140, 210, 0);
                    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(cus.BankAccount, font), 347, 210, 0);
                    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(cus.BillingAddress, font), 140, 195, 0);
                    ColumnText.ShowTextAligned(cb2, Element.ALIGN_CENTER, new Phrase(cus.CustomerId.ToString(), font), 424, 250, 0);
                    pdfStamper.Close();
                    pdfReader.Close();
                    await _PContractSvc.updatePContractFile(int.Parse(id_Pcontract),outputPdfFile);
                }
                    if(id_Pcontract != null)
                    {
                        return Ok (new{
                            retText = "Thêm hợp đồng thành công",
                            data = await _PContractSvc.getPContractAsnyc(int.Parse(id_Pcontract))
                        });
                    }
            }
            return Ok(new {
                reftext = "",
                data = "dữ liệu không hợp lệ"
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPContractAsync(int id)
        {
            return Ok(new{
                retText = "Lấy hợp đồng thành công",
                data = await _PContractSvc.getPContractAsnyc(id)
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPContractsAsync()
        {
            return Ok(new{
                retText = "Lấy danh sách hợp đồng thành công",
                data = await _PContractSvc.getPContractsAsnyc()
            });
        }
        // public async Task<IActionResult> UpdatePContractAsync()
        // {

        // }
    }
}