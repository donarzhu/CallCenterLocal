using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenterLocal.Data
{
    public class DialPhoneInfo
    {
        public int id { get; set; }
        public string cust_number { get; set; }
        public string channal_uuid { get; set; }
        public string flow_id { get; set; }
    }
}
