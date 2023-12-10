using System;
using System.ComponentModel.DataAnnotations;
namespace QuanLyHopDongVaKySo.CLIENT.Models.ModelPost
{
    public class PostTMinute
    {
        public IFormFile? File { get; set; }
        public string? TMinuteName { get; set; }    
        public string? jsonInstallerZone { get; set; }
        public string? jsonCustomerZone { get; set; }
        public string? Base64StringFile { get; set; }

    }
}