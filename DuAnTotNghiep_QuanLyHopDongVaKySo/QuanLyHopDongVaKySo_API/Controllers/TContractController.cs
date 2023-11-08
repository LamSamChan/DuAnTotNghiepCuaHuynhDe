
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services;
using QuanLyHopDongVaKySo_API.Services.TemplateContractService;
using QuanLyHopDongVaKySo_API.Helpers;
using Spire.Pdf.General.Find;
using Spire.Pdf;
using System.Drawing;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TContractController:ControllerBase
    {
        private readonly ITemplateContractSvc _TContractSvc;
        private readonly IContractCoordinateSvc _contractCoordinateSvc;
         private readonly IUploadFileHelper _helpers;
        private readonly IPdfToImageHelper _pdfToImgHelpers;
        public TContractController(ITemplateContractSvc TContractSvc,IUploadFileHelper helpers, IContractCoordinateSvc contractCoordinateSvc, IPdfToImageHelper pdfToImgHelpers)
        {
            _TContractSvc = TContractSvc;
            _helpers = helpers;
            _contractCoordinateSvc = contractCoordinateSvc;
            _pdfToImgHelpers = pdfToImgHelpers;
        }

        /// <summary>
        /// Lấy danh sách toàn bộ mẫu hợp đồng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTContractsAsnyc()
        {
            return Ok(new
                {
                    retText = "Lấy danh sách mẫu hợp đồng thành công",
                    data = await _TContractSvc.getAllAsnyc()
                }
            );
        }

        /// <summary>
        /// Lấy mẫu hợp đồng được chọn theo id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTContractAsnyc(int id)
        {
            return Ok(new{
                retText = "Lấy hợp đồng mẫu thành công",
                data = await _TContractSvc.getByIdAsnyc(id)
            });
        }

        /// <summary>
        /// Thêm mẫu hợp đồng
        /// </summary>
        /// <param name="tContract"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddTContractAsnyc(PostTContract tContract)
        {
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
            if (ModelState.IsValid)
             {
                int id_Tcontract = await _TContractSvc.addAsnyc(tContract);
                string filePath = null;
                float X = 0; float Y = 0;
                if(id_Tcontract > 0)
                {
                    if(tContract.Base64StringFile != null)
                    {
                        IFormFile file = _helpers.ConvertBase64ToIFormFile(tContract.Base64StringFile, tContract.TContractName, "application/pdf"); 
                        filePath = _helpers.UploadFile(file, "AppData","TContracts",".pdf");
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
                                PostContractCoordinate coordinate = new PostContractCoordinate() {
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

                        _pdfToImgHelpers.PdfToPng(filePath, id_Tcontract, "tcontract");
                        return Ok(new
                        {
                            retText = "Thêm mẫu hợp đồng thành công",
                            data = _TContractSvc.getByIdAsnyc(id_Tcontract).Result.TContactID.ToString()
                        }); 
                    }
                }
             }
            return Ok(new {
                retText = "dữ liệu không hợp lệ",
                data = ""
            });
        }

        /// <summary>
        /// Sửa mẫu hợp đồng
        /// </summary>
        /// <param name="tContract"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateTContractAsnyc([FromForm] PutTContract tContract)
        {
             if(ModelState.IsValid)
             {
                int id_Tcontract = await _TContractSvc.updateAsnyc(tContract);
                if(id_Tcontract > 0)
                {
                    if(tContract.File != null)
                    {
                        //_helpers.UploadFile(tContract.File,"AppData","TContracts");
                        return Ok (new{
                            retText = "sửa mẫu hợp đồng thành công",
                            data = await _TContractSvc.getByIdAsnyc(id_Tcontract)
                        });
                    }
                }
             }
            return Ok(new {
                retText = "dữ liệu không hợp lệ",
                data = ""
            });
        }
    }
}