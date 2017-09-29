﻿using CefSharp;
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
        public const string server = "http://192.168.199.139:8080";
        public const string URL= "http://192.168.199.139:8080/static/dist/index.html";
        public const int ThreadSleepTime = 2000;
        public const int ThreadShortSleep = 500;
        private ICookieManager mCookieManager;
        //private bool mIsEndCheck = false;
        private bool isExit = false;
        private Token Token = new Token();
        private ChromiumWebBrowser browser;
        public Form1()
        {
            InitializeComponent();
            //Cef.Initialize(new CefSettings());
            var setting = new CefSharp.CefSettings()
            {
                CachePath = Directory.GetCurrentDirectory() + @"\Cache",
            };

            Cef.Initialize(setting);
            browser = new ChromiumWebBrowser(URL);
            //browser = new ChromiumWebBrowser("http://www.baidu.com");
            browser.Height = 300;
            browser.Width = 500;
            this.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;

            BrowserSettings browserSettings = new BrowserSettings();
            browserSettings.FileAccessFromFileUrls = CefState.Enabled;
            browserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            browserSettings.WebSecurity = CefState.Enabled;
            browser.BrowserSettings = browserSettings;
            
            mCookieManager = CefSharp.Cef.GetGlobalCookieManager();
            browser.FrameLoadEnd += Browser_FrameLoadEnd;
            

         }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            //MessageBox.Show("页面载入完成");
            string _url = e.Url;
            //判断是否是需要获取cookie的页面
            if (_url.Contains(server))
            {
                Thread checkThread = new Thread(() =>
                {
                    while (String.IsNullOrEmpty(Token.token)&&!isExit)
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
            visitor.SendCookie += (Cookie obj)=> {
                Token.token = obj.Value;
                //mIsEndCheck = true;
                if(!String.IsNullOrEmpty(Token.token))
                {
                    Thread getDialPhoneManagerThread = new Thread(() =>
                    {
                        while (!this.isExit)
                        {
                            Thread.Sleep(ThreadSleepTime);
                            try
                            {
                                String retString = HttpControl.GetHttpResponseList<DialPhoneInfo>(HttpControl.GetNeedCallPhoneCmd, 50000,Token.token);
                                List<DialPhoneInfo> infos = (List<DialPhoneInfo>)HttpControl.JSONStringToList<DialPhoneInfo>(retString);
                                foreach (DialPhoneInfo info  in infos)
                                {
                                    int id = info.id;
                                }
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
            if(String.IsNullOrEmpty(Token.token))
            {
                MessageBox.Show("请登录后再试！");
                return;
            }
            try
            {
                TestPageDialog dlg = new TestPageDialog();
                Token tempToken = new Token();
                tempToken.token = "a08074ead1a8f3b4ffa42895b28d02938f3aacbf";
                string param = HttpControl.ObjectToJson(tempToken);
                ResultWorkflows getResult = (ResultWorkflows)HttpControl.PostMoths(HttpControl.GetWorkflowCmd, param,Token);
                if (getResult == null || getResult.successful == false)
                    return;
                dlg.Data = getResult;
                dlg.Token = this.Token;
                dlg.ShowDialog();
            }
            catch(Exception ex)
            {
                return;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cef.Shutdown();
            this.isExit = true;
        }
    }

    public class CookieVisitor : CefSharp.ICookieVisitor
    {
        public event Action<CefSharp.Cookie> SendCookie;
        public bool Visit(CefSharp.Cookie cookie, int count, int total, ref bool deleteCookie)
        {
            deleteCookie = true;
            if (SendCookie != null)
            {
                SendCookie(cookie);
            }

            return true;
        }
    }
 }