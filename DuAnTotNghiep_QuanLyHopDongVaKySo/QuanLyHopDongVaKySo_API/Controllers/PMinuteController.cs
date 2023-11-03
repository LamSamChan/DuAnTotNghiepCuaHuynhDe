using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services;
using QuanLyHopDongVaKySo_API.Services.DoneContractService;
using QuanLyHopDongVaKySo_API.Services.InstallationRequirementService;
using QuanLyHopDongVaKySo_API.Services.PendingMinuteService;
using QuanLyHopDongVaKySo_API.Services.TemplateMinuteService;
using QuanLyHopDongVaKySo_API.Services.TypeOfServiceService;
using System.Reflection;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PMinuteController : ControllerBase
    {
        private readonly IPendingMinuteSvc _pMinuteSvc;
        private readonly ITemplateMinuteSvc _tMinuteSvc;
        private readonly IMinuteCoordinateSvc _mCoordinateSvc;
        private readonly ITypeOfServiceSvc _typeOfServiceSvc;
        private readonly IDoneContractSvc _doneContractSvc;
        private readonly IInstallationRequirementSvc _installationRequirementSvc;

        public PMinuteController(IPendingMinuteSvc pMinuteSvc, ITemplateMinuteSvc tMinuteSvc, IMinuteCoordinateSvc mCoordinateSvc,
            ITypeOfServiceSvc typeOfServiceSvc, IDoneContractSvc doneContractSvc, IInstallationRequirementSvc installationRequirementSvc)
        {
            _pMinuteSvc = pMinuteSvc;
            _tMinuteSvc = tMinuteSvc;
            _mCoordinateSvc = mCoordinateSvc;
            _typeOfServiceSvc = typeOfServiceSvc;
            _doneContractSvc = doneContractSvc;
            _installationRequirementSvc = installationRequirementSvc;
        }

        [HttpPost]
        public async Task<IActionResult> GetTaskFormIRequirement(int iRequirementId, string employeeID)
        {
            var ir = await _installationRequirementSvc.GetById(iRequirementId);

            PendingMinute pendingMinute = new PendingMinute()
            {
                DateCreated = DateTime.Now,
                MinuteName = ir.MinuteName,
                IsIntallation = false,
                IsCustomer = false,
                MinuteFile = "",
                DoneContract = ir.DoneContract,
                EmployeeId = Guid.Parse(employeeID)
            };

            int pMinute = await _pMinuteSvc.addAsnyc(pendingMinute);

            var tos = _doneContractSvc.getByIdAsnyc(ir.DoneContractId).Result.TOS_ID;

            var tMinuteId = _typeOfServiceSvc.GetById(tos).Result.TMinuteID;
            // lấy thông tin biên bản mẫu
            var tMinute = await _tMinuteSvc.getByIdAsnyc(tMinuteId);
            //lây thông tin toạ động mẫu biên bản
            var Coordinates = await _mCoordinateSvc.getByTMinute(tMinuteId);
            if (ModelState.IsValid)
            {
                var minuteById = await _pMinuteSvc.GetById(pMinute);
                var minute = await _pMinuteSvc.ExportContract(minuteById, employeeID);
                if (minute != null)
                {
                    if (!Directory.Exists("AppData/PMinutes/"))
                    {
                        Directory.CreateDirectory("AppData/PMinutes/");
                    }
                    string pdfFilePath = tMinute.TMinuteFile;
                    string outputPdfFile = "AppData/PMinutes/" + pMinute + ".pdf";
                    PdfReader pdfReader = new PdfReader(pdfFilePath);
                    PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(outputPdfFile, FileMode.Create));
                    // Tạo một font cho trường văn bản
                    BaseFont bf = BaseFont.CreateFont(@"AppData/texgyretermes-regular.otf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    // Thiết lập font và kích thước cho trường văn bản
                    Font font = new Font(bf, 12);

                    foreach (var coordinate in Coordinates)
                    {
                        string fieldName = coordinate.FieldName; // Tên trường từ bảng toạ độ
                        float x = coordinate.X + 20; // Lấy tọa độ X từ bảng toạ độ
                        float y = 794 - coordinate.Y; // Lấy tọa độ Y từ bảng toạ độ
                        var mappingName = MinuteInfo.MinuteFieldName.FirstOrDefault(id => id.Key == fieldName).Value;
                        if (mappingName == null)
                        {
                            continue;
                        }
                        PropertyInfo property = typeof(MinuteInfo).GetProperty(mappingName);
                        if (property != null)
                        {
                            object value = property.GetValue(minute);
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
                    await _pMinuteSvc.updatePMinuteFile(pMinute, outputPdfFile);
                    await _installationRequirementSvc.DeleteIRequirement(iRequirementId);

                   
                }
            }
            return Ok();
        }
    }
}