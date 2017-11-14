using CefSharp;
using System;
using System.IO;
using System.Windows;

namespace CallCenterForWpf
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        System.Threading.Mutex mutex;
        public App()
        {
            this.Startup += new StartupEventHandler((sender,e)=> {
                bool ret;
                mutex = new System.Threading.Mutex(true, "CallCenterLocalApp", out ret);

                if (!ret)
                {
                    MessageBox.Show("程序已经运行");
                    Environment.Exit(0);
                }

                var settings = new CefSettings()
                {
                    //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                    CachePath = Path.Combine(Directory.GetCurrentDirectory(), "CefSharp\\Cache"),
                    Locale = "zh-CN",
                    LogFile = "/LogData",
                    UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36"
                };
                settings.CefCommandLineArgs.Add("enable-media-stream", "1");
                settings.CefCommandLineArgs.Add("disable-gpu", "1");
                //Perform dependency check to make sure all relevant resources are in our output directory.
                Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            });

        }
    }
}
