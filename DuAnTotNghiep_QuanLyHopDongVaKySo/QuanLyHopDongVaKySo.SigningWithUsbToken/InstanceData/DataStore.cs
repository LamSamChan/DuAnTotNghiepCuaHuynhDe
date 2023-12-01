using QuanLyHopDongVaKySo.SigningWithUsbToken.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHopDongVaKySo.SigningWithUsbToken.InstanceData
{
    public class DataStore
    {
        private static DataStore _instance;

        public Customer Customer { get; set; }
        public string Token { get; set; }
        public PendingContract PendingContract { get; set; }
        public PendingMinute PendingMinute { get; set; }
        public TemplateContract TemplateContract { get; set; }
        public TypeOfService TypeOfService { get; set; }
        public string SavePath { get; set; }
        public string SavePathSign { get; set; }
        private DataStore() { }

        public static DataStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataStore();
                }
                return _instance;
            }
        }
    }

}
