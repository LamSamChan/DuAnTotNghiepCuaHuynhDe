
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
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
        public PMinuteController (IPendingMinuteSvc pMinute, ITemplateMinuteSvc _tMinute, IMinuteCoordinateSvc mCoordinateSvc)
        {
            _pMinuteSvc = pMinute;
            _tMinute = _tMinute;
            _mCoordinateSvc = mCoordinateSvc;
        }

        // public async Task<IActionResult> AddPMinuteAsnyc(PostPMinute pMinute)
        // {
        //     // lấy thông tin biên bản mẫu
        //     var tMinute = _tMinuteSvc.getTMinuteAsnyc(pMinute.TMinuteId);
        //     //lây thông tin toạ động mẫu biên bản
        //     var Coordinates = await _mCoordinateSvc.getByTMinute(pMinute.TMinuteId);
        //     if(ModelState.IsValid)
        //     {
        //         //
        //         string id_Pminute = await _pMinuteSvc.addAsnyc(pMinute);
        //         var minuteById = await _pMinuteSvc.GetById(int.Parse(id_Pminute));
        //         var minute = await _mCoordinateSvc.
        //     }
        //     return Ok;
        // }
    }
}