using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenterLocal.Data
{
    public class TestWorkflow
    {
        public String numbers { set; get; }
        public String flow_id { set; get; }
    }
    
    //public class TestWorkflowResultData
    //{
    //    public String cust_number { get; set; }
    //    public String channal_uuid { get; set; }
    //    public String flow_id { get; set; }
    //    public String id { get; set; }
    //}

    public class TestWorkflowResult
    {
        public int status { get; set; }
        public DialPhoneInfo[] result { set; get; }
    }

    public class TestResultError
    {
        public string state { get; set; }
        public string error { get; set; }
    }
}
