using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PutTContract
    {
        public int TContractID {get;set;}
        public string TContractName { get; set; }
        public string TContractFile{ get; set; }
        public string jsonDirectorZone { get; set; }
        public string jsonCustomerZone { get; set; }
    }
}