using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services;
using QuanLyHopDongVaKySo_API.Services.TemplateContractService;
using QuanLyHopDongVaKySo_API.Helpers;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Services.TemplateMinuteService;
using Microsoft.AspNetCore.Http.HttpResults;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TMinuteController:ControllerBase
    {
        private readonly ITemplateMinuteSvc _TMinuteSvc;
        private readonly IUploadFileHelper _helpers;
        public TMinuteController(ITemplateMinuteSvc TMinuteSvc,IUploadFileHelper helpers)
        {
            _TMinuteSvc = TMinuteSvc;
            _helpers = helpers;
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
            if(ModelState.IsValid)
            {
                int id_Tminute = await _TMinuteSvc.addAsnyc(tMinute);

                if(id_Tminute >0)
                {
                    if(tMinute.File != null)
                    {
                        _helpers.UploadFile(tMinute.File,"AppData","TMinutes");
                        return Ok(new {
                        retText = "Thêm mẫu biên bản thành công",
                        data = await _TMinuteSvc.getByIdAsnyc(id_Tminute)
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
                        _helpers.UploadFile(tMinute.File,"AppData","TMinutes");
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