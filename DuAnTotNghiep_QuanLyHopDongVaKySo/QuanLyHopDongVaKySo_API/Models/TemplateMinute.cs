﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class TemplateMinute
    {
        [Key]
        [Column("Id")]
        public int TMinuteID { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm:ss dd/MM/yyyy}")]
        [Display(Name = "Ngày thêm")]
        public DateTime DateAdded { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Tên biên bản")]
        [Required(ErrorMessage = "Hãy điền tên biên bản !")]
        public string TMinuteName { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Tệp biên bản")]
        public string TMinuteFile { get; set; }

        public string jsonIntallationZone { get; set; }
        public string jsonCustomerZone { get; set; }

        //tạo liên kết
        [JsonIgnore]
        public ICollection<MinuteCoordinate>? MinuteCoordinates { get; set; }
    }
}
