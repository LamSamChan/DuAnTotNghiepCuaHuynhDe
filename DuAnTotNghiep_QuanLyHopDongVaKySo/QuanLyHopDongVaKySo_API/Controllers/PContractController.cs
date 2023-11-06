using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ContractInfo;
using QuanLyHopDongVaKySo_API.Services;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.PendingContractService;
using QuanLyHopDongVaKySo_API.Services.TemplateContractService;
using QuanLyHopDongVaKySo_API.Services.TypeOfServiceService;
using QuanLyHopDongVaKySo_API.Services.DoneContractService;
using System.Reflection;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PContractController : ControllerBase
    {
        private readonly IPendingContractSvc _PContractSvc;
        private readonly ICustomerSvc _CustomerSvc;
        private readonly ITemplateContractSvc _TContractSvc;
        private readonly IContractCoordinateSvc _CCoordinateSvc;
        private readonly IPdfToImageHelper _pdfToImageHelper;
        private readonly ITypeOfServiceSvc _typeOfServiceSvc;
        private readonly IDoneContractSvc _doneContractSvc;
        public PContractController(IPendingContractSvc PContractSvc, ICustomerSvc CustomerSvc,
         ITemplateContractSvc TContractSvc, IContractCoordinateSvc CCoordinateSvc, IPdfToImageHelper pdfToImageHelper,
         ITypeOfServiceSvc typeOfServiceSvc, IDoneContractSvc doneContractSvc
            )
        {
            _PContractSvc = PContractSvc;
            _CustomerSvc = CustomerSvc;
            _TContractSvc = TContractSvc;
            _CCoordinateSvc = CCoordinateSvc;
            _pdfToImageHelper = pdfToImageHelper;
            _typeOfServiceSvc = typeOfServiceSvc;
            _doneContractSvc = doneContractSvc;
        }

        [HttpPost]
        public async Task<IActionResult> AddPContractAsnyc(PostPendingContract pContract)
        {
            var tContractID = _typeOfServiceSvc.GetById(pContract.TOS_ID).Result.templateContractID;
            //lây thông tin mẫu hợp đồng
            var tContract = await _TContractSvc.getByIdAsnyc(tContractID);
            //lây thông tin toạ động mẫu hợp đòng
            var Coordinates = await _CCoordinateSvc.getByTContract(tContractID);
            List<string> outputPathContracts = new List<string>();
            if (ModelState.IsValid)
            {
                Employee emp = new Employee();
                //thêm hợp đồng
                string id_Pcontract = await _PContractSvc.addAsnyc(pContract);

                var contractById = await _PContractSvc.getByIdAsnyc(int.Parse(id_Pcontract));

                var contract = await _PContractSvc.ExportContract(contractById, emp);
                if (contract != null)
                {
                    if (!Directory.Exists($"AppData/PContracts/{id_Pcontract}"))
                    {
                        Directory.CreateDirectory($"AppData/PContracts/{id_Pcontract}");
                    }
                    string pdfFilePath = tContract.TContractFile;
                    string outputPdfFile = $"AppData/PContracts/{id_Pcontract}/" + id_Pcontract + ".pdf";

                    PdfReader pdfReader = new PdfReader(pdfFilePath);
                    PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(outputPdfFile, FileMode.Create));
                    // Tạo một font cho trường văn bản
                    BaseFont bf = BaseFont.CreateFont(@"AppData/texgyretermes-regular.otf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    // Thiết lập font và kích thước cho trường văn bản
                    Font font = new Font(bf, 10);

                    foreach (var coordinate in Coordinates)
                    {
                        string fieldName = coordinate.FieldName; // Tên trường từ bảng toạ độ
                        float x = coordinate.X + 22; // Lấy tọa độ X từ bảng toạ độ
                        float y = 837 - coordinate.Y; // Lấy tọa độ Y từ bảng toạ độ
                        var mappingName = ContractInternet.ContractFieldName.FirstOrDefault(id => id.Key == fieldName).Value;
                        if (mappingName == null)
                        {
                            continue;
                        }
                        PropertyInfo property = typeof(ContractInternet).GetProperty(mappingName);
                        if (property != null)
                        {
                            object value = property.GetValue(contract);
                            if (value != null)
                            {
                                string contractValue = value.ToString();
                                ColumnText.ShowTextAligned(pdfStamper.GetOverContent(coordinate.SignaturePage),
                                Element.ALIGN_BASELINE, new Phrase(contractValue, font), x, y, 0);
                            }
                        }
                    }

                    pdfStamper.Close();
                    pdfReader.Close();
                    outputPathContracts = _pdfToImageHelper.PdfToPng(outputPdfFile, int.Parse(id_Pcontract), "contract");

                    FileStream fsPContract = new System.IO.FileStream(outputPdfFile, FileMode.Open, FileAccess.Read);
                    fsPContract.Close();
                    await _PContractSvc.updatePContractFile(int.Parse(id_Pcontract), outputPdfFile);
                }
                if (id_Pcontract != null)
                {
                    return Ok(new
                    {
                        retText = "Thêm hợp đồng thành công",
                        data = await _PContractSvc.getByIdAsnyc(int.Parse(id_Pcontract)),
                        imgae = outputPathContracts
                    }); ;
                }
            }
            return Ok(new
            {
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
            var pcontract = await _PContractSvc.getByIdAsnyc(id);
            if (pcontract != null)
            {
                return Ok(pcontract);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPContractsAsync()
        {
            var pContractList = await _PContractSvc.getAllAsnyc();
            if (pContractList != null)
            {
                return Ok(pContractList);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("WaitDirectorSigns")]
        public async Task<IActionResult> GetListWaitDirectorSigns()
        {
            return Ok(await _PContractSvc.getListWaitDirectorSigns());
        }


        [HttpGet("WaitCustomerSigns")]
        public async Task<IActionResult> GetListWaitCustomerSigns()
        {
            return Ok(await _PContractSvc.getListWaitCustomerSigns());
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(PutPendingContract pContract)
        {
            return Ok(await _PContractSvc.updateAsnyc(pContract));
        }
    }
}