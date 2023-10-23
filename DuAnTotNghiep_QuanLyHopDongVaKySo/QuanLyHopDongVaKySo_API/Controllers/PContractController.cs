using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.PendingContractService;
using QuanLyHopDongVaKySo_API.Services.TemplateContractService;
using QuanLyHopDongVaKySo_API.Helpers;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Data.Common;
using QuanLyHopDongVaKySo_API.Services;
using Microsoft.AspNetCore.Http.Features;
namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PContractController:ControllerBase
    {
        private readonly IPendingContractSvc _PContractSvc;
        private readonly ICustomerSvc _CustomerSvc;
        private readonly ITemplateContractSvc _TContractSvc;
        private readonly IContractCoordinateSvc _CCoordinateSvc;

        public PContractController(IPendingContractSvc PContractSvc, ICustomerSvc CustomerSvc, ITemplateContractSvc TContractSvc, IContractCoordinateSvc CCoordinateSvc)
        {
            _PContractSvc = PContractSvc;
            _CustomerSvc = CustomerSvc;
            _TContractSvc = TContractSvc;
            _CCoordinateSvc = CCoordinateSvc;
        }

        [HttpPost]
        public async Task<IActionResult> AddPContractAsnyc([FromForm] PostPendingContract PContract)
        {
            var cus = await _CustomerSvc.GetByIdAsync(PContract.CustomerId.ToString());
            var tContract =await _TContractSvc.getTContractAsnyc(PContract.TContractId);
            var Coordinates = await _CCoordinateSvc.getByTContract(PContract.TContractId);
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
                    string[] fielNames = {"MaKH","MaHD","NgayTao","HoTen","NgaySinh","CCCC","NgayCap","NoiCap","NganHang","SoTK","MaSoThue"};
                    // Thiết lập font và kích thước cho trường văn bản
                    Font font = new Font(bf, 12);
                    foreach (var item in Coordinates)
                    {
                        if(item.FieldName == "MaKH")
                        {
                            ColumnText.ShowTextAligned(pdfStamper.GetOverContent(item.SignaturePage),
                            Element.ALIGN_BASELINE, new Phrase(cus.CustomerId.ToString(), font), (float)item.X, (float)item.Y, 0);
                        }
                        if(item.FieldName == "MaHD")
                        {
                            ColumnText.ShowTextAligned(pdfStamper.GetOverContent(item.SignaturePage),
                            Element.ALIGN_BASELINE, new Phrase(id_Pcontract, font), (float)item.X, (float)item.Y, 0);
                        }
                        if(item.FieldName == "NgayTao")
                        {
                            ColumnText.ShowTextAligned(pdfStamper.GetOverContent(item.SignaturePage),
                            Element.ALIGN_BASELINE, new Phrase(DateTime.Now.Date.ToString("dd/MM/yyyy"), font), (float)item.X, (float)item.Y, 0);
                        }
                        if(item.FieldName == "HoTen")
                        {
                            ColumnText.ShowTextAligned(pdfStamper.GetOverContent(item.SignaturePage),
                            Element.ALIGN_BASELINE, new Phrase(cus.FullName, font), (float)item.X, (float)item.Y, 0);
                        }
                        if(item.FieldName == "NgaySinh")
                        {
                            ColumnText.ShowTextAligned(pdfStamper.GetOverContent(item.SignaturePage),
                            Element.ALIGN_BASELINE, new Phrase(cus.DateOfBirth.ToString("dd/MM/yyyy"), font), (float)item.X, (float)item.Y, 0);
                        }
                        if(item.FieldName == "CCCC")
                        {
                            ColumnText.ShowTextAligned(pdfStamper.GetOverContent(item.SignaturePage),
                            Element.ALIGN_BASELINE, new Phrase(cus.Identification, font), (float)item.X, (float)item.Y, 0);
                        }
                        if(item.FieldName == "NgayCap")
                        {
                            ColumnText.ShowTextAligned(pdfStamper.GetOverContent(item.SignaturePage),
                            Element.ALIGN_BASELINE, new Phrase(cus.IssuedDate.ToString("dd/MM/yyyy"), font), (float)item.X, (float)item.Y, 0);
                        }
                        if(item.FieldName == "NoiCap")
                        {
                            ColumnText.ShowTextAligned(pdfStamper.GetOverContent(item.SignaturePage),
                            Element.ALIGN_BASELINE, new Phrase(cus.IssuedPlace, font), (float)item.X, (float)item.Y, 0);
                        }
                        if(item.FieldName == "NganHang")
                        {
                            ColumnText.ShowTextAligned(pdfStamper.GetOverContent(item.SignaturePage),
                            Element.ALIGN_BASELINE, new Phrase(cus.BankName, font), (float)item.X, (float)item.Y, 0);
                        }
                        if(item.FieldName == "SoTK")
                        {
                            ColumnText.ShowTextAligned(pdfStamper.GetOverContent(item.SignaturePage),
                            Element.ALIGN_BASELINE, new Phrase(cus.BankAccount, font), (float)item.X, (float)item.Y, 0);
                        }
                        if(item.FieldName == "MaSoThue")
                        {
                            ColumnText.ShowTextAligned(pdfStamper.GetOverContent(item.SignaturePage),
                            Element.ALIGN_BASELINE, new Phrase(cus.Address, font), (float)item.X, (float)item.Y, 0);
                        }
                    }
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
