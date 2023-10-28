using System.ComponentModel.DataAnnotations;
namespace QuanLyHopDongVaKySo_API.Models
{
    public class PostMinuteCoordinate
    {
        [Required]
        public string FieldName { get; set; }
        [Required]
        public float X { get; set; }
        [Required]
        public float Y { get; set; }
        [Required]
        public int SignaturePage {get;set;}
        [Required]
        public int TMinutetID { get; set; }
    }
}