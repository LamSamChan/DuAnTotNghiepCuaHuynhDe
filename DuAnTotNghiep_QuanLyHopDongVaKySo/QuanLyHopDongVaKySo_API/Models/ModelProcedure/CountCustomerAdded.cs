﻿namespace QuanLyHopDongVaKySo_API.Models.ModelProcedure
{
    public class CountCustomerAdded
    {
        public DateTime Date { get; set; }
        public int TotalCustomers { get; set; }
        public int EnterpriseCustomers { get; set; }
        public int IndividualCustomers { get; set; }
    }
}
