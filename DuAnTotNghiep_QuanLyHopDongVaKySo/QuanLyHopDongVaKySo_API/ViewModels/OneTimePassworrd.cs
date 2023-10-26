using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyHopDongVaKySo_API.ViewModels
{
    public class OneTimePassworrd
    {
        [MaxLength(8)]
        [Required(ErrorMessage ="Hãy nhập mã xác nhận !")]
        public string Otp { get; set; }

        [Compare("Otp", ErrorMessage = "Mã xác nhận không đúng !")]
        [Display(Name = "Mã xác nhận")]
        public string ConfirmOtp { get; set; }
    }
}
