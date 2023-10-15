using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PostTContract
    {
        [Required]
        public string TContractName { get; set; }
        [Required]
        public string TContractFile{ get; set; }
        [Required]
        public string jsonDirectorZone { get; set; }
        [Required]
        public string jsonCustomerZone { get; set; }
        [Required]
        public IFormFile File {get;set;}
    }
}