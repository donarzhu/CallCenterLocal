using CallCenterLocal.Control;
using CallCenterLocal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CallCenterForWpf
{
    /// <summary>
    /// TestPage.xaml 的交互逻辑
    /// </summary>
    public partial class TestPage : Page
    {
        CallPhoneControl phoneControl = new CallPhoneControl();
        public TestPage()
        {
            InitializeComponent();
            GetData();
        }

        public void GetData()
        {
            try
            {
                ResultWorkflows getResult = PageCommon.GetWorkflows(MainWindow.main.Token);
                if (getResult == null || getResult.successful == false)
                    return;
                PageCommon.SetFlowCombom(getResult, WorkflowCombo);
            }
            catch (Exception ex)
            {
                return;
            }
        }
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadVoiceDialog dlg = new DownloadVoiceDialog();
            dlg.ShowDialog();
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            String strPhone1 = this.PhoneEdit.Text;
            String workflowName = this.WorkflowCombo.Text;
            if (String.IsNullOrEmpty(strPhone1) || strPhone1.Length < 6)
            {
                MessageBox.Show("请输入正确的电话号码1！");
                return;
            }
            if (String.IsNullOrEmpty(workflowName))
            {
                MessageBox.Show("请选择工作流程！");
                return;
            }
            String workflowId = PageCommon.Dict[workflowName];
            if (!String.IsNullOrEmpty(workflowId))
            {
                TestWorkflow flow = new TestWorkflow();
                flow.flow_id = workflowId;
                flow.numbers = strPhone1;
                string param = HttpControl.ObjectToJson(flow);
                string cmd = HttpControl.TestWorkflowCmd + "?" + "flow_id=" + flow.flow_id + "&numbers=" + flow.numbers;
                String strResult = HttpControl.GetHttpResponseList(cmd, 5000, MainWindow.main.Token.TokenCode);
                TestWorkflowResult result = (TestWorkflowResult)HttpControl.JsonToObject<TestWorkflowResult>(strResult);
                if (result == null)
                {
                    MessageBox.Show("网络连接失败！");
                    return;
                }
                if (result.status == 0)
                {
                    MessageBox.Show("测试发送成功。");
                }
                phoneControl.startDialPstn(result.result, MainWindow.main.Token.TokenCode);

            }
        }

        private void PhoneEdit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");

            e.Handled = re.IsMatch(e.Text);

        }
    }
}
