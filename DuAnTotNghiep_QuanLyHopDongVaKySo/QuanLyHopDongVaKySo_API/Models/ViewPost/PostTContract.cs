using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PostTContract
    {
        public IFormFile? File { get; set; }
        public string? TContractName { get; set; }
        public string? jsonDirectorZone { get; set; }

        public string? jsonCustomerZone { get; set; }
        
        public string? Base64StringFile { get; set; }
    }
}