using System;
using System.ComponentModel.DataAnnotations;
namespace QuanLyHopDongVaKySo_API.Models
{
    public class PostTMinute
    {
        [Required]
        public string TMinuteName { get; set; }
        
        public IFormFile? File {get;set;}
        [Required]
        public string Base64StringFile { get; set; }
        [Required]
        public string jsonDirectorZone { get; set; }
        [Required]
        public string jsonCustomerZone { get; set; }
    }
}