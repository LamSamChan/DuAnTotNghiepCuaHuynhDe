using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace QuanLyHopDongVaKySo.SigningWithUsbTokenI.Models
{
    public class DoneMinute
    {
        public string DMinuteName { get; set; }
        public string? Base64File { get; set; }
        public int PMinuteID { get; set; }
    }
}
