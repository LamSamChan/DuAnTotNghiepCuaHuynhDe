using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace QuanLyHopDongVaKySo.CLIENT.Models.ModelPost
{
    public class PostDContract_Usb
    {
        public string DConTractName { get; set; }
        public string? Base64File { get; set; }
        public int PContractID { get; set; }
    }
}
