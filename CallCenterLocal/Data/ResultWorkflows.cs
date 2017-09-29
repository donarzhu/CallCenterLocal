using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenterLocal.Data
{
    public class ResultWorkflows
    {
        public string message { get; set; } = "";
        public bool successful { get; set; } = false;
        public Workflow[] data { get; set; }=null;
    }

    public class Workflow
    {
        public int account_id { get; set; }
        public string content { get; set; }
        public DateTime create_time { get; set; }
        public string id { get; set; }
        public string main_templet_id { get; set; }
        public string title { get; set; }
    }
}
