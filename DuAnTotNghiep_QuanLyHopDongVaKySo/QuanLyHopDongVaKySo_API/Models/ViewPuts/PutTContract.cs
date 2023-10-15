using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PutTContract
    {
        [Required]
        public int TContractID {get;set;}
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