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
            string base64String = null;
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
                        float y = 839 - coordinate.Y; // Lấy tọa độ Y từ bảng toạ độ
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

                    BaseFont bf2 = BaseFont.CreateFont(@"AppData/texgyretermes-bold.otf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    // Thiết lập font và kích thước cho trường văn bản
                    Font font2 = new Font(bf2, 15);


                    foreach (var coordinate in Coordinates)
                    {
                        string fieldName = coordinate.FieldName; // Tên trường từ bảng toạ độ
                        float x = coordinate.X + 35; // Lấy tọa độ X từ bảng toạ độ
                        float y = 834 - coordinate.Y; // Lấy tọa độ Y từ bảng toạ độ
                        var mappingName = ContractInternet.CustomerSignNameInfo.FirstOrDefault(id => id.Key == fieldName).Value;
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
                                string contractValue = value.ToString().ToUpper();
                                ColumnText.ShowTextAligned(pdfStamper.GetOverContent(coordinate.SignaturePage),
                                Element.ALIGN_BASELINE, new Phrase(contractValue, font2), x, y, 0);
                            }
                        }
                    }

                    pdfStamper.Close();
                    pdfReader.Close();
                    //outputPathContracts = _pdfToImageHelper.PdfToPng(outputPdfFile, int.Parse(id_Pcontract), "contract");

                    FileStream fsPContract = new System.IO.FileStream(outputPdfFile, FileMode.Open);
                    fsPContract.Close();
                   
                    byte[] fileBytes = System.IO.File.ReadAllBytes(outputPdfFile);
                    base64String = Convert.ToBase64String(fileBytes);

                    await _PContractSvc.updatePContractFile(int.Parse(id_Pcontract), outputPdfFile, base64String);
                }
                if (id_Pcontract != null)
                {
                    return Ok(base64String+"*"+id_Pcontract); 
                }
            }
            return BadRequest();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPContractAsync(int id)
        {    
            return Ok(await _PContractSvc.geByIdView(id));
            
        }
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPContractsAsync()
        {
            return Ok(await _PContractSvc.getAllAsnyc());            
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
        public async Task<IActionResult> Update([FromBody] PutPendingContract pContract)
        {
            var respone = await _PContractSvc.updateAsnyc(pContract);
            if (respone != null)
            {
                return Ok();
            }
            return BadRequest();

        }

        [HttpGet("GetByEmpId/{id}")]
        public async Task<IActionResult> GetListByEmpId(string id)
        {
            return Ok(await _PContractSvc.getListEmpId(id));
        }
        [HttpGet("GetByCusId/{id}")]
        public async Task<IActionResult> GetListByCusId(string id)
        {
            return Ok(await _PContractSvc.getListCusId(id));
        }
        [HttpGet("GetRefuseByEmpId/{id}")]
        public async Task<IActionResult> GetListRefuseByEmpId(string id)
        {
            return Ok(await _PContractSvc.getListRefuseByEmpId(id));
        }
        [HttpGet("GetRefuse")]
        public async Task<IActionResult> GetRefuse()
        {
            return Ok(await _PContractSvc.getListRefuse());
        }
        [HttpGet("GetWaitCusSignsByEmpId/{id}")]
        public async Task<IActionResult> GetListWaitCusSignsByEmpId(string id)
        {
            return Ok(await _PContractSvc.getListWaitCusSignsByEmpId(id));
        }
        [HttpGet("GetWaitCusSignsByDirId/{id}")]
        public async Task<IActionResult> GetListWaitCusSignsByDirId(string id)
        {
            return Ok(await _PContractSvc.getListWaitCusSignsByDirId(id));
        }

        [HttpGet("GetWaitDirSignsEmpId/{id}")]
        public async Task<IActionResult> GetListWaitDirSignsEmpId(string id)
        {
            return Ok(await _PContractSvc.getListWaitDirSignsByEmpId(id));
        }
    }
}