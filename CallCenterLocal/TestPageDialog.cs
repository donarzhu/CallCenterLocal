using CallCenterLocal.Control;
using CallCenterLocal.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CallCenterLocal
{
    public partial class TestPageDialog : Form
    {
        CallPhoneControl phoneControl = new CallPhoneControl();
        public TestPageDialog()
        {
            InitializeComponent();
        } 

        private void TestButton_Click(object sender, EventArgs e)
        {
            String strPhone1 = this.mPhoneOneTextBox.Text;
            String strPhone2 = this.mPhoneTowTextBox.Text;
            String workflowName = this.mWorkflowComboBox.Text;
            if(String.IsNullOrEmpty(strPhone1) || strPhone1.Length<6)
            {
                MessageBox.Show("请输入正确的电话号码1！");
                return;
            }
            if (!String.IsNullOrEmpty(strPhone2))
            {
                if(strPhone2.Length<6)
                {
                    MessageBox.Show("请输入正确的电话号码2！");
                    return;
                }
            }
            if (String.IsNullOrEmpty(workflowName))
            {
                MessageBox.Show("请选择工作流程！");
                return;
            }
            String workflowId = _dict[workflowName];
            if(!String.IsNullOrEmpty(workflowId))
            {
                TestWorkflow flow = new TestWorkflow();
                flow.flow_id = workflowId;
                flow.numbers = strPhone1;
                if(!String.IsNullOrEmpty(strPhone2))
                {
                    flow.numbers += "," + strPhone2;
                }
                string param = HttpControl.ObjectToJson(flow);
                string cmd = HttpControl.TestWorkflowCmd + "?" + "flow_id=" + flow.flow_id + "&numbers=" + flow.numbers;
                String strResult = HttpControl.GetHttpResponseList(cmd, 5000 ,this.Token.token);
                TestWorkflowResult result = (TestWorkflowResult)HttpControl.JsonToObject<TestWorkflowResult>(strResult);
                if(result == null)
                {
                    MessageBox.Show("网络连接失败！");
                    return;
                }
                if(result.status==0)
                {
                    MessageBox.Show("测试发送成功。");
                }
                phoneControl.startDialPstn(result.result, this.Token.token);


            }
        }

        private void mPhoneOneTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))//如果不是输入数字就不让输入
            {
                e.Handled = true;
            }
        }

        private void mPhoneTowTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))//如果不是输入数字就不让输入
            {
                e.Handled = true;
            }

        }

        private void TestPageDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
