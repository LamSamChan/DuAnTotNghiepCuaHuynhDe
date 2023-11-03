using System.ComponentModel.DataAnnotations;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class MinuteInfo
    {
        static public Dictionary<string, string> MinuteFieldName = new Dictionary<string, string>
        {
            { "1", "MinuteCreatedDate" },
            { "2", "MinuteContent" },
            { "3", "MinuteId" },
            { "4", "ContractId" },
            { "5", "InstallationCustomer" },
            { "6", "CustomerName" },
            { "7", "CustomerPosition" },
            { "8", "CustomerPhone" },
            { "9", "InstallationCompany" },
            { "10", "InstallationPerson" },
            { "11", "InstallationPosition" },
            { "12", "InstallationPhone" },
            { "14", "InstallationAddress" },
            { "15", "Number1" },
            { "16", "FirstDevice" },
            { "17", "DeviceStatus1" },
            { "18", "Quantity1" },
            { "19", "Number2" },
            { "20", "SecondDevice" },
            { "21", "DeviceStatus2" },
            { "22", "Quantity2" },
            { "23", "Number3" },
            { "24", "ThirdDevice" },
            { "25", "DeviceStatus3" },
            { "26", "Quantity3" },
            { "27", "Number4" },
            { "28", "FourthDevice" },
            { "29", "DeviceStatus4" },
            { "30", "Quantity4" },

        };

        static public Dictionary<string, string> Installation = new Dictionary<string, string>
        {
            { "13", "InstallationDate" },
        };

        public string MinuteCreatedDate { get; set; }

        [Required(ErrorMessage = "Hãy nhập nội dung của biên bản !")]
        [Display(Name = "Nội dung biên bản lắp đặt")]
        public string MinuteContent { get; set; }

        [Required(ErrorMessage = "Hãy nhập mã biên bản !")]
        [Display(Name = "Mã biên bản")]
        public string MinuteId { get; set; }

        [Required(ErrorMessage = "Hãy nhập mã hợp đồng !")]
        [Display(Name = "Mã hợp đồng")]
        public string ContractId { get; set; }

        [Required(ErrorMessage = "Hãy nhập tên công ty lắp đặt !")]
        [Display(Name = "Bên B")]
        public string InstallationCompany { get; set; }

        [Required(ErrorMessage = "Hãy nhập họ tên người lắp đặt !")]
        [Display(Name = "Họ và tên")]
        public string InstallationPerson { get; set; }

        [Required(ErrorMessage = "Hãy nhập chức vụ người lắp đặt !")]
        [Display(Name = "Chức vụ")]
        public string InstallationPosition { get; set; }

        [Required(ErrorMessage = "Hãy nhập số điện thoại người lắp đặt !")]
        [Display(Name = "Số điện thoại")]
        public string InstallationPhone { get; set; }

        [Required(ErrorMessage = "Hãy nhập nơi được lắp đặt !")]
        [Display(Name = "Bên A")]
        public string InstallationCustomer { get; set; }

        [Required(ErrorMessage = "Hãy nhập họ tên người đại diện nơi lắp đặt !")]
        [Display(Name = "Họ và tên")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Hãy nhập chức vụ người đại diện nơi lắp đặt !")]
        [Display(Name = "Chức vụ")]
        public string CustomerPosition { get; set; }

        [Required(ErrorMessage = "Hãy nhập số điện thoại người lắp đặt !")]
        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "Hãy nhập ngày lắp đặt !")]
        [Display(Name = "Ngày")]
        public string? InstallationDate { get; set; }

        [Required(ErrorMessage = "Hãy nhập địa chỉ lắp đặt !")]
        [Display(Name = "Địa chỉ lắp đặt")]
        public string InstallationAddress { get; set; }

        [Display(Name = "Số thứ tự 1")]
        public int? Number1 { get; set; }

        [Display(Name = "Thiết bị thứ nhất")]
        public string? FirstDevice { get; set; }
        public string? DeviceStatus1 { get; set; }
        [Display(Name = "Số lượng thiết bị thứ nhất")]
        public int? Quantity1 { get; set; }

        [Display(Name = "Số thứ tự 2")]
        public int? Number2 { get; set; }
        [Display(Name = "Thiết bị thứ hai")]
        public string? SecondDevice { get; set; }
        public string? DeviceStatus2 { get; set; }
        [Display(Name = "Số lượng thiết bị thứ hai")]
        public int? Quantity2 { get; set; }

        [Display(Name = "Số thứ tự 3")]
        public int? Number3 { get; set; }
        [Display(Name = "Thiết bị thứ ba")]
        public string? ThirdDevice { get; set; }
        public string? DeviceStatus3 { get; set; }

        [Display(Name = "Số lượng thiết bị thứ ba")]
        public int? Quantity3 { get; set; }

        [Display(Name = "Số thứ tự 4")]
        public int? Number4 { get; set; }
        [Display(Name = "Thiết bị thứ tư")]
        public string? FourthDevice { get; set; }
        public string? DeviceStatus4 { get; set; }
        [Display(Name = "Số lượng thiết bị thứ tư")]
        public int? Quantity4 { get; set; }
    }
}
