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
using Microsoft.AspNetCore.Authorization;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            return Ok(await _TMinuteSvc.getAllAsnyc());
        }

        /// <summary>
        /// Lấy mẫu biên bản
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTMinuteAsnyc(int id)
        {
            return Ok(await _TMinuteSvc.getByIdAsnyc(id));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tMinute"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddTMinuteAsnyc(PostTMinute tMinute)
        {
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
            if (ModelState.IsValid)
            {
                var tMinuteExist = _TMinuteSvc.getAllAsnyc().Result.FirstOrDefault(t => t.TMinuteName == tMinute.TMinuteName);
                if (tMinuteExist != null)
                {
                    return BadRequest();
                }
                int id_Tminute = await _TMinuteSvc.addAsnyc(tMinute);
                string filePath = null;
                float X = 0; float Y = 0;
                if (id_Tminute >0)
                {
                    if(tMinute.Base64StringFile != null)
                    {
                         
                        IFormFile file = _helpers.ConvertBase64ToIFormFile(tMinute.Base64StringFile, tMinute.TMinuteName, "application/pdf");
                        filePath = _helpers.UploadFile(file, "AppData", "TMinutes", ".pdf");
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
                        return Ok(id_Tminute);
                    }
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tMinute"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateTMinuteAsnyc([FromBody] PutTMinute tMinute)
        {
            if(ModelState.IsValid)
            {
                int id_Tminute = await _TMinuteSvc.updateAsnyc(tMinute);
                if(id_Tminute >0)
                {
                        return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteTMinute(int id)
        {

            var respone = await _TMinuteSvc.deleteAsnyc(id);
            if (respone)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }
    }
}