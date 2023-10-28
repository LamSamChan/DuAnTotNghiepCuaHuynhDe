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
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models.ContractInfo;
using System.Diagnostics.Contracts;
using System.Reflection;
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
        public PContractController(IPendingContractSvc PContractSvc, ICustomerSvc CustomerSvc,
         ITemplateContractSvc TContractSvc, IContractCoordinateSvc CCoordinateSvc,ProjectDbContext context)
        {
            _PContractSvc = PContractSvc;
            _CustomerSvc = CustomerSvc;
            _TContractSvc = TContractSvc;
            _CCoordinateSvc = CCoordinateSvc;
        }

        [HttpPost]
        public async Task<IActionResult> AddPContractAsnyc([FromForm] PostPendingContract pContract)
        {
            //lây thông tin mẫu hợp đồng
            var tContract =await _TContractSvc.getTContractAsnyc(pContract.TContractId);
            //lây thông tin toạ động mẫu hợp đòng
            var Coordinates = await _CCoordinateSvc.getByTContract(pContract.TContractId);
            if(ModelState.IsValid)
            {
                //thêm hợp đồng
                string id_Pcontract = await _PContractSvc.addPContractAsnyc(pContract);
                var contractById = await _PContractSvc.getPContractAsnyc(int.Parse(id_Pcontract));
                var contract = await _PContractSvc.ExportContract(contractById);
                if(contract != null)
                {
                    if (!Directory.Exists("AppData/PContracts/"))
                    {
                        Directory.CreateDirectory("AppData/PContracts/");
                    }
                    string pdfFilePath = tContract.TContractFile ;
                    string outputPdfFile = "AppData/PContracts/"+id_Pcontract+".pdf";
                    PdfReader pdfReader = new PdfReader(pdfFilePath);
                    PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(outputPdfFile, FileMode.Create));
                    // Tạo một font cho trường văn bản
                    BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);
                    // Thiết lập font và kích thước cho trường văn bản
                    Font font = new Font(bf, 12);
                    
                    foreach(var coordinate in Coordinates)
                    {
                        string fieldName = coordinate.FieldName; // Tên trường từ bảng toạ độ
                        float x = coordinate.X; // Lấy tọa độ X từ bảng toạ độ
                        float y = coordinate.Y; // Lấy tọa độ Y từ bảng toạ độ
                        PropertyInfo property = typeof(ContractInternet).GetProperty(fieldName);
                        if (property != null)
                        {
                            object value = property.GetValue(contract);
                            if (value != null)
                            {
                                string contractValue = value.ToString();
                                ColumnText.ShowTextAligned(pdfStamper.GetOverContent(coordinate.SignaturePage),
                                Element.ALIGN_BASELINE, new Phrase(contractValue, font), (float)coordinate.X, (float)coordinate.Y, 0);
                            }
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
