using CallCenterLocal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CallCenterLocal.Control
{
    public class PageCommon
    {
        static public Dictionary<String, String> Dict { get; private set; } = new Dictionary<string, string>();
        public static ResultWorkflows GetWorkflows(Token token)
        {
            GetWorkflowData data = new GetWorkflowData
            {
                token = token.TokenCode
            };
            var param = HttpControl.ObjectToJson(data);
            var getResult = HttpControl.PostMoths(HttpControl.GetWorkflowCmd, param, token);
            return (ResultWorkflows)getResult;
        }

        public static void SetFlowCombom(ResultWorkflows getResult, ComboBox WorkflowCombo )
        {
            foreach (Workflow info in getResult.data)
            {
                try
                {
                    Dict.Add(info.title, info.id);
                    WorkflowCombo.Items.Add(info.title);
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }
    }


}
