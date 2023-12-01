namespace QuanLyHopDongVaKySo.CLIENT.Constants
{
    public class SessionKey
    {
        public static class Employee
        {
            public const string EmployeeID = "EmployeeID";
            public const string EmployeeContext = "EmployeeContext";
            public const string Role = "Role";
        }

        public static class Customer
        {
            public const string CustomerID = "CustomerID";
            public const string CustomerContext = "CusContext";
            public const string CustomerToken = "CustomerToken";

        }

        public static class PFXCertificate
        {
            public const string Serial = "Serial";
        }

        public static class PedningContract
        {
            public const string PContractID = "PContractID";
        }

        public static class PedningMinute
        {
            public const string PMinuteID = "PMinuteID";
        }
    }
}