using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyHopDongVaKySo.CLIENT.Models.ModelPut
{
    public class PutTMinute
    {
        public int TMinuteID {get;set;}
        public string TMinuteName { get; set; }
        public string TMinuteFile{ get; set; }
        public string jsonInstallerZone { get; set; }
        public string jsonCustomerZone { get; set; }
    }
}