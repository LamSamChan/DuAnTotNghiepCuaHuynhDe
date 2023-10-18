using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services;
using QuanLyHopDongVaKySo_API.Services.TemplateContractService;

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

        public TMinuteController(ITemplateMinuteSvc TMinuteSvc)
        {
            _TMinuteSvc = TMinuteSvc;
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
                data = await _TMinuteSvc.getTMinutesAsnyc()
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
                data = await _TMinuteSvc.getTMinuteAsnyc(id)
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
                int id_Tminute = await _TMinuteSvc.addTMinuteAsnyc(tMinute);

                if(id_Tminute >0)
                {
                    return Ok(new {
                    retText = "Thêm mẫu biên bản thành công",
                    data = await _TMinuteSvc.getTMinuteAsnyc(id_Tminute)
                    });
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
                int id_Tminute = await _TMinuteSvc.updateTMinuteAsnyc(tMinute);
                if(id_Tminute >0)
                {
                    return Ok(new{
                        retText = "Sữa mâu biên bản thành công",
                        data = await _TMinuteSvc.getTMinuteAsnyc(id_Tminute)
                    });
                }
            }
            return Ok(new{
                retText = "Dữ liệu không hợp lệ",
                data = ""
            });
        }
    }
}