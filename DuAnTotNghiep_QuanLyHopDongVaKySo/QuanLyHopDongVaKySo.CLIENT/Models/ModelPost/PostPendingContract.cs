using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo.CLIENT.Models.ModelPost
{
    public class PostPendingContract
    {
        public Guid EmployeeCreatedId { get; set; }
        public Guid CustomerId { get; set; }
        public string InstallationAddress { get; set; }
        public int TOS_ID { get; set; }
    }

}
