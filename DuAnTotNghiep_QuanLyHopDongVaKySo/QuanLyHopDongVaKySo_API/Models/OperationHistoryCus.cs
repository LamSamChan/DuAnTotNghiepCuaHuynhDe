﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class OperationHistoryCus
    {
        [Key]
        [Column("Id")]
        public int HistoryID { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm:ss dd/MM/yyyy}")]
        [Display(Name = "Ngày")]
        public DateTime Date { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        [Display(Name = "Tên thao tác")]
        public string OperationName { get; set; }

        //tạo liên kết
        [ForeignKey("Customer")]
        public Guid CustomerID { get; set; }

        public Customer? Customer { get; set; }
    }
}