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
                string cmd = HttpControl.GeUrlInfoCmd + flow.flow_id + "/bot/";
                String strResult = HttpControl.GetHttpResponseList(cmd, 500, this.Token.token);
                ResultFtpInfo ret = (ResultFtpInfo)HttpControl.JsonToObject<ResultFtpInfo>(strResult);
                if (strResult == null)
                {
                    MessageBox.Show("网络连接失败！");
                    return;
                }
                if(ret.message != "成功")
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
                        HttpManager.HttpDownloadFile(ret.data.url, objFullFileName, new UZipProgressDelegate((count, total)=>{
                            this.BeginInvoke(new InvokeDelegate(() =>
                            {
                                downSize = total;
                                fileSize = count;
                                DownloadInfoTextBox.Text = "正在下载文件:"+ count.ToString() + "/" + total.ToString();
                            }));
                        }));
                        if(downSize <= fileSize)
                        {
                            this.BeginInvoke(new InvokeDelegate(() =>
                            {
                                DownloadInfoTextBox.Text = "文件下载失败";
                            }));
                            return;
                        }
                        ZipHelper.UnZip2(objFullFileName, filePath, new UZipProgressDelegate((total, count) => {
                            this.BeginInvoke(new InvokeDelegate(() =>
                            {
                                DownloadInfoTextBox.Text = "解压文件进度"+total.ToString() + "/" + count.ToString();
                            }));

                        }));
                        MessageBox.Show("下载完成！");
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

        private void FlowComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
