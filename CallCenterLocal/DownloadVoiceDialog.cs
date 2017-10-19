using CallCenterLocal.Control;
using CallCenterLocal.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CallCenterLocal
{
    public partial class DownloadVoiceDialog : Form
    {
        private ResultWorkflows _data = null;
        private Dictionary<String, String> _dict = new Dictionary<string, string>();
        public Token Token { get; set; }
        bool isNewDownload = true;
        int downloadIndex = 0;
        public ResultWorkflows Data
        {
            set { _data = value; LoadData(); }
            get { return _data; }
        }

        private void LoadData()
        {
            if (this.Data == null || this.Data.successful == false)
                return;
            foreach (Workflow info in Data.data)
            {
                try
                {
                    _dict.Add(info.title, info.id);
                    this.FlowComboBox.Items.Add(info.title);
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }

        public DownloadVoiceDialog()
        {
            InitializeComponent();
        }
        private delegate void InvokeDelegate();
        private void DownloadControlButton_Click(object sender, EventArgs e)
        {
            String workflowName = this.FlowComboBox.Text;
            if (String.IsNullOrEmpty(workflowName))
            {
                MessageBox.Show("请选择工作流程！");
                return;
            }
            String workflowId = _dict[workflowName];
            if (!String.IsNullOrEmpty(workflowId))
            {
                TestWorkflow flow = new TestWorkflow();
                flow.flow_id = workflowId;
                string param = HttpControl.ObjectToJson(flow);
                string cmd = HttpControl.GetFtpInfoCmd + flow.flow_id + "/bot/";
                String strResult = HttpControl.GetHttpResponseList<ResultFtpInfo>(cmd, 5000, this.Token.token);
                ResultFtpInfo result = (ResultFtpInfo)HttpControl.JsonToObject<ResultFtpInfo>(strResult);
                if (result == null)
                {
                    MessageBox.Show("网络连接失败！");
                    return;
                }
                if (result.message != "成功")
                    return;
                if (isNewDownload)
                    this.downloadIndex = 0;
                var filePath = CPlayVoicePathManager.GetVoicePath(flow.flow_id);
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        var remotePath = result.data.url.Replace("ftp://" + result.data.ip+"/","");
                        SFTPHelper helper = new SFTPHelper(result.data.ip, "22", result.data.user_name, result.data.password);
                        var list = helper.GetFileList(remotePath,"*.*");
                        for (int i = downloadIndex; i < list.Count; i++)
                        {
                            this.BeginInvoke(new InvokeDelegate(() =>
                            {
                                downloadIndex = i;
                                DownloadInfoTextBox.Text = this.downloadIndex.ToString() + "/" + list.Count.ToString();
                            }));
                            helper.Get(filePath, (String)list[i]);

                        }
                        this.BeginInvoke(new InvokeDelegate(() =>
                        {
                            DownloadInfoTextBox.Text = list.Count.ToString().ToString() + "/" + list.Count.ToString();
                        }));
                    }
                    catch(Exception ex)
                    {
                        var err = ex.Message;
                    }

                });
                thread.Start();

            }
        }

        private void DownloadVoiceDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
