
using System.Diagnostics.Contracts;
using System.Net.NetworkInformation;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ContractInfo;
using QuanLyHopDongVaKySo_API.Services;
using QuanLyHopDongVaKySo_API.Services.PendingMinuteService;
using QuanLyHopDongVaKySo_API.Services.TemplateMinuteService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PMinuteController:ControllerBase
    {
        private readonly IPendingMinuteSvc _pMinuteSvc;
        private readonly ITemplateMinuteSvc _tMinuteSvc;
        private readonly IMinuteCoordinateSvc _mCoordinateSvc;
        public PMinuteController (IPendingMinuteSvc pMinuteSvc, ITemplateMinuteSvc tMinuteSvc, IMinuteCoordinateSvc mCoordinateSvc)
        {
            _pMinuteSvc = pMinuteSvc;
            _tMinuteSvc = tMinuteSvc;
            _mCoordinateSvc = mCoordinateSvc;
        }

        [HttpPost]
         public async Task<IActionResult> AddPMinuteAsnyc([FromForm] PostPMinute pMinute, string empId)
         {
             // lấy thông tin biên bản mẫu
             var tMinute = await _tMinuteSvc.getByIdAsnyc(pMinute.TMinuteId);
             //lây thông tin toạ động mẫu biên bản
             var Coordinates = await _mCoordinateSvc.getByTMinute(pMinute.TMinuteId);
             if(ModelState.IsValid)
             {
                 //
                 string id_Pminute = await _pMinuteSvc.addAsnyc(pMinute);
                 var minuteById = await _pMinuteSvc.GetById(int.Parse(id_Pminute));
                var minute = await _pMinuteSvc.ExportContract(minuteById, empId);
                    if (minute != null)
                {
                    if (!Directory.Exists("AppData/PMinutess/"))
                    {
                        Directory.CreateDirectory("AppData/PMinutess/");
                    }
                    string pdfFilePath = tMinute.TMinuteFile;
                    string outputPdfFile = "AppData/PMinutess/" + id_Pminute + ".pdf";
                    PdfReader pdfReader = new PdfReader(pdfFilePath);
                    PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(outputPdfFile, FileMode.Create));
                    // Tạo một font cho trường văn bản
                    BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);
                    // Thiết lập font và kích thước cho trường văn bản
                    Font font = new Font(bf, 12);

                    foreach (var coordinate in Coordinates)
                    {
                        string fieldName = coordinate.FieldName; // Tên trường từ bảng toạ độ
                        float x = coordinate.X; // Lấy tọa độ X từ bảng toạ độ
                        float y = coordinate.Y; // Lấy tọa độ Y từ bảng toạ độ
                        PropertyInfo property = typeof(MinuteInfo).GetProperty(fieldName);
                        if (property != null)
                        {
                            object value = property.GetValue(minute);
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
                    await _pMinuteSvc.updatePMinuteFile(int.Parse(id_Pminute), outputPdfFile);
                }
                if (id_Pminute != null)
                {
                    return Ok(new
                    {
                        retText = "Thêm hợp đồng thành công",
                        data = await _pMinuteSvc.GetById(int.Parse(id_Pminute))
                    });
                }
            }
            return Ok(new
            {
                reftext = "",
                data = "dữ liệu không hợp lệ"
            });
        }
    }
}