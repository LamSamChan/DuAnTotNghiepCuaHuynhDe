using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PostTContract
    {
        [Required]
        public string TContractName { get; set; }
        [Required]
        public string jsonDirectorZone { get; set; }
        [Required]
        public string jsonCustomerZone { get; set; }
        
        public IFormFile? File {get;set;}
        public string Base64StringFile { get; set; }
    }
}