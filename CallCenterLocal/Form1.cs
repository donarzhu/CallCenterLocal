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

namespace CallCenterLocal
{
    public partial class Form1 : Form
    {
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
            browser = new ChromiumWebBrowser("http://121.42.36.138:8000/static/dist/index.html#/Login");
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

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
