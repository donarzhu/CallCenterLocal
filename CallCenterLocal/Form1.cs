using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CallCenterLocal.Control;
using System.Web;
using System.Threading;
using System.Reactive;
using System.Reactive.Threading;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.PlatformServices;
using CallCenterLocal.Data;

namespace CallCenterLocal
{
    public partial class Form1 : Form
    {
        public const string server = "http://114.215.68.77:8000";
        public const string URL = "https://ccc.aicyber.com/static/dist/index.html";//"https://webcamtoy.com";//"https://114.215.68.77/static/record.html";//"http://114.215.68.77:8000/static/dist/index.html";
        const int topSpan = 60;
        public const int ThreadSleepTime = 2000;
        public const int ThreadShortSleep = 500;
        private ICookieManager mCookieManager;
        //private bool mIsEndCheck = false;
        private bool isExit = false;
        private Token Token = new Token();
        private ChromiumWebBrowser browser;
        private CallPhoneControl phoneControl = new CallPhoneControl();
        frmWaitingBox waitingBox = null;
        private Form1 my = null;
        public Form1()
        {
            InitializeComponent();
            my = this;
            phoneControl.openDevInit();
            //Cef.Initialize(new CefSettings());
            var setting = new CefSharp.CefSettings()
            {
                CachePath = Directory.GetCurrentDirectory() + @"\Cache",
            };
            setting.CefCommandLineArgs.Add("enable-media-stream", "1");
            //setting.CefCommandLineArgs.Add("--disable-web-security", "1"); 

            Cef.Initialize(setting);
            browser = new ChromiumWebBrowser(URL);
            //browser = new ChromiumWebBrowser("http://www.baidu.com");
            browser.Height = this.Height - topSpan;
            browser.Width = this.Width;
            this.Controls.Add(browser);
            browser.Dock = DockStyle.Bottom;

            BrowserSettings browserSettings = new BrowserSettings();
            browserSettings.FileAccessFromFileUrls = CefState.Enabled;
            browserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            browserSettings.WebSecurity = CefState.Enabled;
            browser.BrowserSettings = browserSettings;

            mCookieManager = CefSharp.Cef.GetGlobalCookieManager();
            browser.FrameLoadEnd += Browser_FrameLoadEnd;

            waitingBox = new frmWaitingBox((obj, args) =>
            {

            });
            waitingBox.StartPosition = FormStartPosition.CenterScreen;
            waitingBox.Show(this);
        }
        private delegate void InvokeDelegate();
        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            //MessageBox.Show("页面载入完成");
            my.BeginInvoke(new InvokeDelegate(() =>
            {
                waitingBox.Hide();
                waitingBox.WindowClose();

            }));
            string _url = e.Url;
            //判断是否是需要获取cookie的页面
            if (_url.Contains(server))
            {
                Thread checkThread = new Thread(() =>
                {
                    while (String.IsNullOrEmpty(Token.token) && !isExit)
                    {
                        //mIsEndCheck = false;
                        Set_CookieVisitor();
                        Thread.Sleep(ThreadSleepTime);
                    }

                });
                checkThread.Start();
            }
        }

        void Set_CookieVisitor()
        {
            //注册获取cookie回调事件
            CookieVisitor visitor = new CookieVisitor();
            visitor.SendCookie += (Cookie obj) => {
                if (obj.Name != "auth_t")
                    return;
                Token.token = obj.Value;
                //mIsEndCheck = true;
                if (!String.IsNullOrEmpty(Token.token))
                {
                    Thread getDialPhoneManagerThread = new Thread(() =>
                    {
                        while (!this.isExit)
                        {
                            Thread.Sleep(ThreadSleepTime);
                            try
                            {
                                String retString = HttpControl.GetHttpResponseList(HttpControl.GetNeedCallPhoneCmd, 50000, Token.token);
                                List<DialPhoneInfo> infos = (List<DialPhoneInfo>)HttpControl.JSONStringToList<DialPhoneInfo>(retString);
                                DialPhoneInfo[] dialInfos = new DialPhoneInfo[infos.Count];
                                int i = 0;
                                foreach (DialPhoneInfo info in infos)
                                {
                                    dialInfos[i] = info;
                                    i++;
                                }
                                phoneControl.startDialPstn(dialInfos, this.Token.token);
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                    });
                    getDialPhoneManagerThread.Start();
                }
            }; ;
            mCookieManager.VisitAllCookies(visitor);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void TestPageStripMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Token.token))
            {
                MessageBox.Show("请登录后再试！");
                return;
            }
            try
            {
                TestPageDialog dlg = new TestPageDialog();
                ResultWorkflows getResult = GetWorkflows(Token);
                if (getResult == null || getResult.successful == false)
                    return;
                dlg.Data = getResult;
                dlg.Token = this.Token;
                dlg.ShowDialog();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private ResultWorkflows GetWorkflows(Token token)
        {
            //Token tempToken = new Token();
            //tempToken.token = "208dbf9f968a432815a2b726ed49de0a669b5f27";//"a08074ead1a8f3b4ffa42895b28d02938f3aacbf";
            GetWorkflowData data = new GetWorkflowData();
            data.token = token.token;
            var param = HttpControl.ObjectToJson(data);
            var getResult = HttpControl.PostMoths(HttpControl.GetWorkflowCmd, param, Token);
            return (ResultWorkflows)getResult;
        }

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            phoneControl.closeDev();
            Cef.Shutdown();
            this.isExit = true;
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
        }

        private void DownloadMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Token.token))
            {
                MessageBox.Show("请登录后再试！");
                return;
            }
            Token tempToken = new Token();
            tempToken.token = Token.token;//"a08074ead1a8f3b4ffa42895b28d02938f3aacbf";
            ResultWorkflows getResult = GetWorkflows(Token);
            if (getResult == null || getResult.successful == false)
                return;
            DownloadVoiceDialog dlg = new DownloadVoiceDialog();
            dlg.Data = getResult;
            dlg.Token = tempToken;
            dlg.ShowDialog();

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            browser.Height = this.Height - topSpan;
            browser.Width = this.Width;
            browser.Dock = DockStyle.Bottom;
        }
    }

    public class CookieVisitor : CefSharp.ICookieVisitor
    {
        public event Action<CefSharp.Cookie> SendCookie;
        public bool Visit(CefSharp.Cookie cookie, int count, int total, ref bool deleteCookie)
        {
            deleteCookie = false;
            if (SendCookie != null)
            {
                SendCookie(cookie);
            }

            return true;
        }
    }


 }
