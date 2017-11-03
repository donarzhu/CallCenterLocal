using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenterLocal.Data
{
    public class Ftpinfo
    {
        public String url { get; set; }
        public String ip { get; set; }
        public String password { get; set; }
        public String user_name { get; set; }
        public int port { get; set; }
    }
    public class ResultFtpInfo
    {
        public String status { get; set; }
        public String message { get; set; }
        public Ftpinfo data { get; set; }
    }
}
