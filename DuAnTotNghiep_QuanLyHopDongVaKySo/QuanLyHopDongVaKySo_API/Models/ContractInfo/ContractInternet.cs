using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo_API.Models.ContractInfo
{
    
    public class ContractInternet
    {
        static public Dictionary<string, string> ContractFieldName = new Dictionary<string, string>
        {
            { "1", "CustomerId" },
            { "2", "ContractId" },
            { "3", "Date" },
            { "4", "BuisinessName" },
            { "5", "FullName" },
            { "6", "Position" },
            { "7", "DateOfBirth" },
            { "8", "BuisinessNumber" },
            { "9", "BNDate" },
            { "10", "BNPlace" },
            { "11", "Identification" },
            { "12", "IssuedDate" },
            { "13", "IssuedPlace" },
            { "14", "PowerOfAttorneyNum" },
            { "15", "DatePOA" },
            { "16", "WhoPOA" },
            { "17", "BankAccount" },
            { "18", "BankName" },
            { "19", "TaxIDNumber" },
            { "20", "Address" },
            { "21", "ChargeNoticeAddress" },
            { "22", "BillingAddress" },
            { "23", "PhoneNumber" },
            { "24", "FAX" },
            { "25", "Email" },
            { "28", "Username" },
            { "29", "TariffPackage" },
            { "30", "InstallationAddress" },
            { "31", "ServiceRate" }
        };

        static public Dictionary<string, string> RepresentativeContract = new Dictionary<string, string>
        {
            { "26", "RepresentativePerson1" },
            { "27", "RepresentativePosition1" },
            { "32", "RepresentativePosition2" },
            { "33", "RepresentativePerson2" },

        };

        static public Dictionary<string, string> CustomerSignNameInfo = new Dictionary<string, string>
        {
            { "34", "CustomerSignPosition" },
            { "35", "CustomerSignName" },
        };

        [Required(ErrorMessage = "Hãy điền mã khách hàng !")]
        [Display(Name = "Mã khách hàng")]
        public string CustomerId { get; set; }

        [Required(ErrorMessage = "Hãy điền mã hợp đồng !")]
        [Display(Name = "Mã hợp đồng")]
        public string ContractId { get; set; }

        [Required(ErrorMessage = "Hãy điền ngày!")]
        [Display(Name = "Ngày")]
        public string Date { get; set; }

        [Display(Name = "Tên doanh nghiệp")]
        public string? BuisinessName { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Hãy điền họ và tên !")]
        public string FullName { get; set; }

        [Display(Name = "Chức vụ")]
        public string? Position { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Hãy chọn ngày,tháng,năm sinh !")]
        [Display(Name = "Năm sinh")]
        public string DateOfBirth { get; set; }

        public string? BuisinessNumber { get; set; }
        public string? BNDate { get; set; }
        public string? BNPlace { get; set; }

        // [Required(ErrorMessage = "Hãy chọn giới tính !"),
        //     Range(1, 2, ErrorMessage = "Giới tính không hợp lệ !")]
        // [Display(Name = "Giới tính")]
        // public Gender Gender { get; set; }

        [Required(ErrorMessage = "Hãy nhập số điện thoại !")]
        [RegularExpression("^(?:\\+84|0)\\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ !")]
        [Display(Name = "Số điện thoại")]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "varchar(100)"), StringLength(50)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Hãy điền email !")]
        [RegularExpression("^[a-zA-Z0-9]+(?:\\.[a-zA-Z0-9]+)*@[a-zA-Z0-9]+(?:\\.[a-zA-Z0-9]+)*$",
           ErrorMessage = "Email không hợp lệ !")]
        public string Email { get; set; }

        [Display(Name = "FAX")]
        [MaxLength(50)]
        public string? FAX { get; set; }

        [Required(ErrorMessage = "Hãy số chứng minh nhân dân / căn cước công dân !")]
        [Display(Name = "CMND/CCCD")]
        [MaxLength(20)]
        public string Identification { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Hãy chọn ngày,tháng,năm cấp CMND/CCCD!")]
        [Display(Name = "Ngày cấp")]
        public string IssuedDate { get; set; }

        [Required(ErrorMessage = "Hãy nhập nơi cấp CMND/CCCD!")]
        [Display(Name = "Nơi cấp")]
        public string IssuedPlace { get; set; }

        [Required(ErrorMessage = "Hãy nhập Quốc Tịch!")]
        [Display(Name = "Quốc tịch")]
        public string Nationality { get; set; }

        [Display(Name = "Giấy uỷ quyền")]
        [MaxLength(20)]
        public string? PowerOfAttorneyNum { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày")]
        public string? DatePOA { get; set; }

        [Display(Name = "Của")]
        public string? WhoPOA { get; set; }

        [Required(ErrorMessage = "Hãy nhập số tài khoản ngân hàng!")]
        [Display(Name = "Số tài khoản ngân hàng")]
        public string BankAccount { get; set; }

        [Required(ErrorMessage = "Hãy nhập tên ngân hàng!")]
        [Display(Name = "Tên ngân hàng")]
        public string BankName { get; set; }

        [Display(Name = "Mã số thuế")]
        public string? TaxIDNumber { get; set; }

        [Required(ErrorMessage = "Hãy nhập địa chỉ thường chú / giao dịch!")]
        [Display(Name = "Địa chỉ thường chú / giao dịch")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Hãy nhập địa gửi giấy báo cước !")]
        [Display(Name = "Địa chỉ gửi giấy báo cước")]
        public string ChargeNoticeAddress { get; set; }

        [Required(ErrorMessage = "Hãy nhập địa gửi giấy báo cước !")]
        [Display(Name = "Địa chỉ thanh toán")]
        public string BillingAddress { get; set; }

        [Required(ErrorMessage = "Hãy nhập trên truy nhập !")]
        [Display(Name = "Tên truy nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Hãy nhập tên gói cước !")]
        [Display(Name = "Gói cước")]
        public string TariffPackage { get; set; }

        [Required(ErrorMessage = "Hãy nhập giá cước dịch vụ !")]
        [Display(Name = "Giá dịch cước vụ")]
        public string ServiceRate { get; set; }

        [Required(ErrorMessage = "Hãy nhập địa xuất lắp đặt !")]
        [Display(Name = "Địa chỉ lắp đặt")]
        public string InstallationAddress { get; set; }

        [Display(Name = "Người đại diện")]
        public string? RepresentativePerson1 { get; set; }
        public string? RepresentativePerson2 { get; set; }
        [Display(Name = "Chức vụ người đại diện")]
        public string? RepresentativePosition1 { get; set; }
        public string? RepresentativePosition2 { get; set; }
        public string? CustomerSignName { get; set; }
        public string? CustomerSignPosition { get; set; }
    }
}

