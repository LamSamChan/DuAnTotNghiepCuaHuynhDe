using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PutTMinute
    {
        [Required]
        public int TMinuteID {get;set;}
        [Required]
        public string TMinuteName { get; set; }
        [Required]
        public string TMinuteFile{ get; set; }
        [Required]
        public string jsonDirectorZone { get; set; }
        [Required]
        public string jsonCustomerZone { get; set; }
        [Required]
        public IFormFile File {get;set;}
    }
}