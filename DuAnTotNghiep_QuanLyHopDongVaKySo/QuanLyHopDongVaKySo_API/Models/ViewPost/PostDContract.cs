using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PostDContract
    {
        public string DConTractName { get; set; }
        public string? Base64File { get; set; }
        public int PContractID { get; set; }
    }
}
