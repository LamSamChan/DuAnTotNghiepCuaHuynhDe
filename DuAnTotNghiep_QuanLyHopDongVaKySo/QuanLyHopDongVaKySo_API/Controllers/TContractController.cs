using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services;
using QuanLyHopDongVaKySo_API.Services.TemplateContractService;
using Spire.Pdf;
using Spire.Pdf.General.Find;
using System.Drawing;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TContractController : ControllerBase
    {
        private readonly ITemplateContractSvc _TContractSvc;
        private readonly IContractCoordinateSvc _contractCoordinateSvc;
        private readonly IUploadFileHelper _helpers;

        public TContractController(ITemplateContractSvc TContractSvc, IUploadFileHelper helpers, IContractCoordinateSvc contractCoordinateSvc)
        {
            _TContractSvc = TContractSvc;
            _helpers = helpers;
            _contractCoordinateSvc = contractCoordinateSvc;
        }

        /// <summary>
        /// Lấy danh sách toàn bộ mẫu hợp đồng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTContractsAsnyc()
        {
            return Ok(await _TContractSvc.getAllAsnyc());
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteTContract(int id)
        {

            var respone = await _TContractSvc.deleteAsnyc(id);
            if (respone)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
            
        }

        /// <summary>
        /// Lấy mẫu hợp đồng được chọn theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTContractAsnyc(int id)
        {
            return Ok( await _TContractSvc.getByIdAsnyc(id));
        }
        /// <summary>
        /// Thêm mẫu hợp đồng
        /// </summary>
        /// <param name="tContract"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddTContractAsnyc([FromBody] PostTContract tContract)
        {
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
            if (ModelState.IsValid)
            {
                var tContractExist = _TContractSvc.getAllAsnyc().Result.FirstOrDefault(t => t.TContractName == tContract.TContractName);
                if (tContractExist != null)
                {
                    return BadRequest();
                }
                int id_Tcontract = await _TContractSvc.addAsnyc(tContract);
                string filePath = null;
                float X = 0; float Y = 0;
                if (id_Tcontract > 0)
                {
                    if (tContract.Base64StringFile != null)
                    {
                        IFormFile file = _helpers.ConvertBase64ToIFormFile(tContract.Base64StringFile, tContract.TContractName, "application/pdf");
                        filePath = _helpers.UploadFile(file, "AppData", "TContracts", ".pdf");
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
                                PostContractCoordinate coordinate = new PostContractCoordinate()
                                {
                                    FieldName = $"{i}",
                                    X = X,
                                    Y = Y,
                                    SignaturePage = pageNum,
                                    TContractID = id_Tcontract,
                                };
                                await _contractCoordinateSvc.add(coordinate);
                            }
                            if (bearkForeach)
                            {
                                break;
                            }
                        }
                        return Ok(id_Tcontract);
                    }
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Sửa mẫu hợp đồng
        /// </summary>
        /// <param name="tContract"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateTContractAsnyc([FromBody] PutTContract tContract)
        {
            if (ModelState.IsValid)
            {
                int id_Tcontract = await _TContractSvc.updateAsnyc(tContract);
                if (id_Tcontract != 0)
                {
                    return Ok();
                }
                return BadRequest();
                    
                
            }
            return BadRequest(new
            {
                retText = "dữ liệu không hợp lệ",
                data = ""
            });
        }
    }
}