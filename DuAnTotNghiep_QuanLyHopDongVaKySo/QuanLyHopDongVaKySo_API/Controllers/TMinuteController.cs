using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services;
using QuanLyHopDongVaKySo_API.Services.TemplateContractService;
using QuanLyHopDongVaKySo_API.Helpers;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Services.TemplateMinuteService;
using Microsoft.AspNetCore.Http.HttpResults;
using Spire.Pdf.General.Find;
using Spire.Pdf;
using System.Drawing;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TMinuteController:ControllerBase
    {
        private readonly ITemplateMinuteSvc _TMinuteSvc;
        private readonly IUploadFileHelper _helpers;
        private readonly IMinuteCoordinateSvc _mCoordinateSvc;
        public TMinuteController(ITemplateMinuteSvc TMinuteSvc,IUploadFileHelper helpers, IMinuteCoordinateSvc mCoordinateSvc)
        {
            _TMinuteSvc = TMinuteSvc;
            _helpers = helpers;
            _mCoordinateSvc = mCoordinateSvc;
        }

        /// <summary>
        /// Lấy danh sach mẫu biên bản
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTMinutesAsnyc()
        {
            return Ok(new{
                retText = "Lấy danh sách mẫu biên bản thành công",
                data = await _TMinuteSvc.getAllAsnyc()
            });
        }

        /// <summary>
        /// Lấy mẫu biên bản
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTMinuteAsnyc(int id)
        {
            return Ok(new{
                retText = "Lấy mẫu biên bản thành công",
                data = await _TMinuteSvc.getByIdAsnyc(id)
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tMinute"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddTMinuteAsnyc([FromForm] PostTMinute tMinute)
        {
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
            if (ModelState.IsValid)
            {
                int id_Tminute = await _TMinuteSvc.addAsnyc(tMinute);
                string filePath = null;
                float X = 0; float Y = 0;
                if (id_Tminute >0)
                {
                    if(tMinute.File != null)
                    {
                        /*filePath = _helpers.UploadFile(tMinute.File,"AppData","TMinutes");*/
                        int pageNum = 0;
                        doc.LoadFromFile(filePath);

                        int i = 1;
                        bool bearkForeach = false;
                        foreach (PdfPageBase page in doc.Pages)
                        {
                            pageNum++;
                            for (; i <= 50; i++)
                            {
                                PdfTextFind[] results = null;
                                results = page.FindText($"({i})", TextFindParameter.IgnoreCase).Finds;

                                if (results.Length == 0)
                                {
                                    if (pageNum == doc.Pages.Count)
                                    {
                                        bearkForeach = true;
                                    }
                                    break;
                                }
                                foreach (PdfTextFind text in results)
                                {
                                    PointF p = text.Position;
                                    X = p.X;
                                    Y = p.Y;
                                    break;
                                }
                                PostMinuteCoordinate coordinate = new PostMinuteCoordinate()
                                {
                                    FieldName = $"{i}",
                                    X = X,
                                    Y = Y,
                                    SignaturePage = pageNum,
                                    TMinutetID = id_Tminute,
                                };
                                await _mCoordinateSvc.add(coordinate);
                            }
                            if (bearkForeach)
                            {
                                break;
                            }
                        }
                        return Ok(new {
                        retText = "Thêm mẫu biên bản thành công",
                        data = _TMinuteSvc.getByIdAsnyc(id_Tminute).Result.TMinuteID.ToString()
                        });
                    }
                }
            }
            return Ok(new {
                retText = "Dữ liệu không hợp lê",
                data = ""
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tMinute"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateTMinuteAsnyc([FromForm] PutTMinute tMinute)
        {
            if(ModelState.IsValid)
            {
                int id_Tminute = await _TMinuteSvc.updateAsnyc(tMinute);
                if(id_Tminute >0)
                {
                    if(tMinute.File != null)
                    {
                        /*_helpers.UploadFile(tMinute.File,"AppData","TMinutes");*/
                        return Ok(new{
                            retText = "Sữa mâu biên bản thành công",
                            data = await _TMinuteSvc.getByIdAsnyc(id_Tminute)
                        });
                    }
                }
            }
            return Ok(new{
                retText = "Dữ liệu không hợp lệ",
                data = ""
            });
        }
    }
}