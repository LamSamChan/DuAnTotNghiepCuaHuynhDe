﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class TypeOfService
    {
        [Key]
        [Column("Id")]
        public int TOS_ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm:ss dd/MM/yyyy}")]
        [Display(Name = "Ngày tạo")]
        public DateTime? DateAdded { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm:ss dd/MM/yyyy}")]
        [Display(Name = "Ngày cập nhật")]
        public DateTime? DateUpdated { get; set; }

        [Required(ErrorMessage = "Hãy điền tên loại dịch vụ !")]
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Tên loại dịch vụ")]
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "Hãy điền giá dịch vụ !")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Hãy điền đơn vị thời gian của giá !")]
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Đơn vị thời gian")]
        public string PerTime { get; set; }

        public bool isHidden { get; set; }

        //tạo liên kết



    }
}
