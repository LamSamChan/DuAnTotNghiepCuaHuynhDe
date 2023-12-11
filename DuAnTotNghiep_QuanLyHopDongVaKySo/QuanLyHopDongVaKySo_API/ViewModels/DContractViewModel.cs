namespace QuanLyHopDongVaKySo_API.ViewModels
{
    public class DContractViewModel 
    {
        public string Id { get; set; }
        public string DContractName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string DateDone { get; set; }
        public string DateUnEffect { get; set; }

        public string TypeOfService { get; set; }
        public string Status { get; set; }
        public string EmployeeCreatedId { get; set; }
        public string DirectorSignedId { get; set; }
        public string CustomerId { get; set; }
        public string TOS_id { get; set; }
        public string? DMinuteID { get; set; }
        public string? Base64File { get; set; }

    }
}
