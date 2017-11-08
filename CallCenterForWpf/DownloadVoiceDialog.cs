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

namespace CallCenterForWpf
{
    public partial class DownloadVoiceDialog : Form
    {
        bool _isTaskCreate = false;
        ToolTip buttontTt = new ToolTip();
        public Boolean IsTaskCreatUse {
            get { return _isTaskCreate; }
            set { _isTaskCreate = value;
                CloseButton.Text = "选择流程";
                if(_isTaskCreate)
                    buttontTt.SetToolTip(CloseButton, "选择流程并关闭窗口");
            }
        }
        public String ShowText { get { return LabText.Text; }
            set { LabText.Text = value; }
        }
        public String SelectWorkflowID { get; private set; }
        private ResultWorkflows _data = null;
        public Token Token { get; set; } = MainWindow.main.Token;
        bool isNewDownload = true;
        int downloadIndex = 0;
        

        public DownloadVoiceDialog()
        {
            InitializeComponent();
            foreach(String workflowName in PageCommon.Dict.Keys)
            {
                FlowComboBox.Items.Add(workflowName);
            }
        }
        private delegate void InvokeDelegate();
        private void DownloadControlButton_Click(object sender, EventArgs e)
        {
            try
            {
                String workflowName = this.FlowComboBox.Text;
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
                    string param = HttpControl.ObjectToJson(flow);
                    string cmd = HttpControl.GeUrlInfoCmd + flow.flow_id + "/bot/";
                    String strResult = HttpControl.GetHttpResponseList(cmd, 500, this.Token.TokenCode);
                    ResultFtpInfo ret = (ResultFtpInfo)HttpControl.JsonToObject<ResultFtpInfo>(strResult);
                    if (strResult == null)
                    {
                        MessageBox.Show("网络连接失败！");
                        return;
                    }
                    if (ret.message != "成功")
                    {
                        MessageBox.Show("网络连接失败！");
                        return;

                    }
                    if (isNewDownload)
                        this.downloadIndex = 0;
                    var filePath = CPlayVoicePathManager.GetVoicePath(flow.flow_id);
                    var objFullFileName = filePath + flow.flow_id.ToString() + ".zip";
                    Thread thread = new Thread(() =>
                    {
                        try
                        {
                            long downSize = 0;
                            long fileSize = 0;
                            HttpManager.HttpDownloadFile(ret.data.url, objFullFileName, new UZipProgressDelegate((count, total) =>
                            {
                                this.BeginInvoke(new InvokeDelegate(() =>
                                {
                                    downSize = total;
                                    fileSize = count;
                                    DownloadInfoTextBox.Text = "正在下载文件:" + count.ToString() + "/" + total.ToString();
                                }));
                            }));
                            if (downSize <= fileSize)
                            {
                                Console.WriteLine("文件下载异常的长度");
                            }
                            ZipHelper.UnZip2(objFullFileName, filePath, new UZipProgressDelegate((total, count) =>
                            {
                                this.BeginInvoke(new InvokeDelegate(() =>
                                {
                                    DownloadInfoTextBox.Text = "解压文件进度" + total.ToString() + "/" + count.ToString();
                                }));

                            }));
                            MessageBox.Show("下载完成！");
                            if(_isTaskCreate)
                            {
                                CloseButton.Text = "关闭窗口";
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    });
                    thread.Start();

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DownloadVoiceDialog_Load(object sender, EventArgs e)
        {

        }

        private void FlowComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsTaskCreatUse)
            {
                String workflowName = this.FlowComboBox.Text;
                if (String.IsNullOrEmpty(workflowName))
                {
                    MessageBox.Show("请选择一个工作流程！");
                }
                else
                {
                    CloseButton.Text = "关闭窗口";
                }
            }
        }

        private void DownloadVoiceDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(IsTaskCreatUse)
            {
                String workflowName = this.FlowComboBox.Text;
                if (String.IsNullOrEmpty(workflowName))
                {
                    MessageBox.Show("请选择工作流程！");
                    e.Cancel = true;
                    return;
                }
                SelectWorkflowID = PageCommon.Dict[workflowName];
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (IsTaskCreatUse)
            {
                String workflowName = this.FlowComboBox.Text;
                if (String.IsNullOrEmpty(workflowName))
                {
                    MessageBox.Show("请选择一个工作流程！");
                    return;
                }
                SelectWorkflowID = PageCommon.Dict[workflowName];
            }
            this.Close();

        }
    }
}
