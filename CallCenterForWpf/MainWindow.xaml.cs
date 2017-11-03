using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Forms;
using CallCenterLocal.Data;
using CallCenterLocal.Control;
using CefSharp;
using System.Threading;

namespace CallCenterForWpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        class PageInfo
        {
            static Dictionary<String, PageInfo> Pages { get; set; } = new Dictionary<string, PageInfo>();
            //delegate void ButtonCommandDelegate(string command);
            //ButtonCommandDelegate ButtonCommand;

            public String Command { get; private set; }
            public Page Page { get; private set; }
            public ImageBrush NormalImage { get; private set; } = null;
            public ImageBrush SelectImage { get; private set; } = null;
            public Boolean IsCurrentPage { get; private set; } = false;
            public System.Windows.Controls.Button CommandButton { get; private set; } = null;
            public String PageUri { get; private set; } = null;
            static String _current = "";
            public static PageInfo CurrentPage {
                get
                {
                    if (_current == "")
                        return null;
                    return Pages[_current];
                }
            }
            public static String CurrentCommand
            {
                get { return _current; }
                set
                {
                    try
                    {
                        PageInfo oldPage = null, currentPage = null;
                        if (_current == value)
                            return;
                        if (_current != "")
                            oldPage = Pages[_current];
                        currentPage = Pages[value];
                        if (currentPage == null)
                            return;
                        if (oldPage != null)
                        {
                            oldPage.CommandButton.Background = oldPage.NormalImage;
                            oldPage.CommandButton.Foreground = new SolidColorBrush(Colors.Black);
                        }
                        currentPage.CommandButton.Background = currentPage.SelectImage;
                        currentPage.CommandButton.Foreground = new SolidColorBrush(Colors.White);
                        _current = value;
                    }
                    catch(Exception e)
                    {
                        String message = e.Message;
                    }

                }
            }

            public PageInfo(String command,Page page,ImageBrush normal,ImageBrush select,System.Windows.Controls.Button button,String uri=null)
            {
                Command = command;
                Page = page;
                NormalImage = normal;
                SelectImage = select;
                CommandButton = button;
                PageUri = uri;
                Pages.Add(command, this);
            }
        }

        public const string loginPageUri = "https://ccc.aicyber.com/static/dist/index.html#";
        public const string repositoryUri = "https://ccc.aicyber.com/static/dist/index.html#/repository";
        public const string taskcreateUri = "https://ccc.aicyber.com/static/dist/index.html#/taskcreate";
        public const string taskeditUri = "https://ccc.aicyber.com/static/dist/index.html#/taskedit";
        public const string resultUri = "https://ccc.aicyber.com/static/dist/index.html#/home/result";
        public const string activecodeUri = "https://ccc.aicyber.com/static/dist/index.html#/activecode";
        public const string blacklistUri = "https://ccc.aicyber.com/static/dist/index.html#/blacklist";
        public const string settingUri = "https://ccc.aicyber.com/static/dist/index.html#/setting";
        public const string statisticsUri = "https://ccc.aicyber.com/static/dist/index.html#/statistics";
        public const string TokenKey = "auth_t";
        public const int ThreadSleepTime = 5000;
        public const int waitThreadSleepTime = 500;
        public bool isExit { get; private set; } = false;
        double SrcWidth { get; set; }
        double SrcHeight { get; set; }
        Boolean IsMax { get; set; } = false;
        DateTime testHitTime = DateTime.Now;
        DateTime AppStartTime { get; } = DateTime.Now;
        public static MainWindow main;
        public Token Token { get; private set; } = new Token();
        private CallPhoneControl phoneControl = new CallPhoneControl();
        private ICookieManager mCookieManager;
        frmWaitingBox waitingBox = null;

        login loginPage { get; set; }
        TestPage TestPage { get; set; }
        LoadPage LoadPage { get; set; }
        //LoadPage WorkflowPage { get; set; }
        //LoadPage TaskCreatePage { get; set; }
        //LoadPage TaskManagerPage { get; set; }
        //LoadPage DataQueryPage { get; set; }
        //LoadPage CodeManagerPage { get; set; }
        //LoadPage BlacklistPage { get; set; }
        //LoadPage SetupPage { get; set; }
        //LoadPage RecordPage { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            main = this;
            mCookieManager = CefSharp.Cef.GetGlobalCookieManager();
            ClearCookie();
            waitingBox = new frmWaitingBox((obj, args) =>
            {

            });
            waitingBox.StartPosition = FormStartPosition.CenterScreen;
            waitingBox.Show();

            tempTest temp = new tempTest();
            //temp.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.Width > SystemParameters.PrimaryScreenWidth)
                this.Width = SystemParameters.PrimaryScreenWidth;
            if(this.Height > SystemParameters.PrimaryScreenHeight)
                this.Height = SystemParameters.PrimaryScreenHeight;
            SrcWidth = this.Width;
            SrcHeight = this.Height;
        }

        private void WindowMax()
        {
            if (IsMax)
                return;
            //获取窗口句柄 
            var handle = new WindowInteropHelper(this).Handle;
            //获取当前显示器屏幕
            Screen screen = Screen.FromHandle(handle);

            this.MaxWidth = screen.Bounds.Width;
            this.MaxHeight = screen.Bounds.Height;
            this.WindowState = WindowState.Maximized;

            ResizePage();
            IsMax = true;
        }

        private void ResizePage()
        {
            if (PageInfo.CurrentPage == null || PageInfo.CurrentPage.Page == null)
                return;
            PageInfo.CurrentPage.Page.Width = main.frame.Width;
            PageInfo.CurrentPage.Page.Height = main.frame.Height;
        }

        private void ExitMaxWindow()
        {
            if (!IsMax)
                return;
            this.MaxWidth = SrcWidth;
            this.MaxHeight = SrcHeight;
            this.WindowState = WindowState.Normal;
            ResizePage();
            IsMax = false;
        }

        public void leftButtonBarCommand(string param)
        {
            main.Dispatcher.Invoke(() => {
                try
                {
                    PageInfo.CurrentCommand = param;
                    PageInfo.CurrentPage.Page.Width = main.frame.ActualWidth;
                    PageInfo.CurrentPage.Page.Height = main.frame.ActualHeight;

                    switch (param)
                    {
                        case "taskCreate":
                            DownloadVoiceDialog dlg = new DownloadVoiceDialog
                            {
                                IsTaskCreatUse = true,
                                ShowText = "如果已经下载流程语音请选择流程后关闭窗口。"
                            };
                            dlg.ShowDialog();
                            String wId = dlg.SelectWorkflowID;
                            var ret = mCookieManager.SetCookieAsync("https://"+Domain, new Cookie()
                            {
                                Domain = Domain, Name = "flow_id", Value = wId, Expires = DateTime.MaxValue
                            });
                            break;
                        default:
                            break;
                    }

                    SetPage(PageInfo.CurrentPage.Page);
                    if(PageInfo.CurrentPage.PageUri != null && PageInfo.CurrentPage.Page is LoadPage)
                    {
                        if(((LoadPage)PageInfo.CurrentPage.Page).Uri != PageInfo.CurrentPage.PageUri)
                            ((LoadPage)PageInfo.CurrentPage.Page).Uri = PageInfo.CurrentPage.PageUri;
                    }

                }
                catch(Exception e)
                {
                    String Messsage = e.Message;
                }
            });
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            var span = DateTime.Now - testHitTime;
            testHitTime = DateTime.Now;
            if (span.Milliseconds <300)
            {
                Console.WriteLine("Now:" + DateTime.Now.ToString() + "TestTime:"+testHitTime.ToString() +"span:"+span.Milliseconds.ToString());
                if (IsMax)
                    ExitMaxWindow();
                else
                    WindowMax();
                testHitTime = AppStartTime;
            }
        }


        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClearCookie();
            isExit = true;
            if (sender == closeButton)
                System.Windows.Application.Current.Shutdown(-1);
        }

        private void ClearCookie()
        {
            mCookieManager.DeleteCookies(Domain, TokenKey);
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public void Browser_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            main.Dispatcher.Invoke(() =>
            {
                waitingBox?.Hide();
                waitingBox?.WindowClose();

            });
            string _url = e.Url;
            //判断是否是需要获取cookie的页面
            if (_url.Contains(loginPageUri))
            {
                Thread checkThread = new Thread(() =>
                {
                    while (String.IsNullOrEmpty(Token.TokenCode) && !isExit)
                    {
                        //mIsEndCheck = false;
                        Set_CookieVisitor();
                        Thread.Sleep(waitThreadSleepTime);
                    }

                });
                checkThread.Start();
            }
        }
        string Domain { get; set; } = "";
        void Set_CookieVisitor()
        {
            //注册获取cookie回调事件
            CookieVisitor visitor = new CookieVisitor();
            visitor.SendCookie += (Cookie obj) => {
                if (obj.Name != TokenKey)
                    return;
                Token.TokenCode = obj.Value;
                Domain = obj.Domain;
                ShowLeftBar();
                StartFistPage();
                //mIsEndCheck = true;
                if (!String.IsNullOrEmpty(Token.TokenCode))
                {
                    Thread getDialPhoneManagerThread = new Thread(() =>
                    {
                        while (!this.isExit)
                        {
                            Thread.Sleep(ThreadSleepTime);
                            try
                            {
                                String retString = HttpControl.GetHttpResponseList(HttpControl.GetNeedCallPhoneCmd, 50000, Token.TokenCode);
                                List<DialPhoneInfo> infos = (List<DialPhoneInfo>)HttpControl.JSONStringToList<DialPhoneInfo>(retString);
                                DialPhoneInfo[] dialInfos = new DialPhoneInfo[infos.Count];
                                int i = 0;
                                foreach (DialPhoneInfo info in infos)
                                {
                                    dialInfos[i] = info;
                                    i++;
                                }
                                phoneControl.startDialPstn(dialInfos, this.Token.TokenCode);
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

        private void StartFistPage()
        {

            main.Dispatcher.Invoke(() =>
            {
                try
                {

                    ImageBrush selectBrush = new ImageBrush { ImageSource = new BitmapImage(new Uri(@"Resources/bar06.png", UriKind.Relative)) };

                     TestPage = new TestPage
                    {

                     };
                    PageInfo testPageInfo = new PageInfo(
                        "testPage",
                        TestPage,
                        (ImageBrush)testPage.Background,
                        selectBrush,
                        testPage,
                        null
                        );
                    SetPage(TestPage);
                    PageInfo.CurrentCommand = testPageInfo.Command;
                    ResizePage();

                    LoadPage = new LoadPage();
                    PageInfo workflowPageInfo = new PageInfo(
                            "workflowEdit",
                            LoadPage,
                            (ImageBrush)workflowEdit.Background,
                            selectBrush,
                            workflowEdit,
                            repositoryUri);
                    PageInfo taskCreatePageInfo = new PageInfo(
                            "taskCreate",
                            LoadPage,
                            (ImageBrush)taskCreate.Background,
                            selectBrush,
                            taskCreate,
                            taskcreateUri);
                    PageInfo taskManagerPage = new PageInfo(
                            "taskManager",
                            LoadPage,
                            (ImageBrush)taskManager.Background,
                            selectBrush,
                            taskManager,
                            taskeditUri);
                    PageInfo vioceRecordPage = new PageInfo(
                            "voiceRecord",
                            LoadPage,
                            (ImageBrush)taskManager.Background,
                            selectBrush,
                            voiceRecord,
                            loginPageUri);
                    PageInfo dataQueryPageInfo = new PageInfo(
                            "dataQuery",
                            LoadPage,
                            (ImageBrush)dataQuery.Background,
                            selectBrush,
                            dataQuery,
                            resultUri);
                    PageInfo statisticsInfo = new PageInfo(
                            "statistics",
                            LoadPage,
                            (ImageBrush)dataQuery.Background,
                            selectBrush,
                            statistics,
                            statisticsUri);
                    PageInfo codeManagerPageInfo = new PageInfo(
                            "codeMagager",
                            LoadPage,
                            (ImageBrush)codeMagager.Background,
                            selectBrush,
                            codeMagager,
                            activecodeUri);
                    PageInfo blacklistPageInfo = new PageInfo(
                            "blacklist",
                            LoadPage,
                            (ImageBrush)blacklist.Background,
                            selectBrush,
                            blacklist,
                            blacklistUri);
                    PageInfo setupPageInfo = new PageInfo(
                            "setup",
                            LoadPage,
                            (ImageBrush)setup.Background,
                            selectBrush,
                            setup,
                            settingUri);

                }
                catch (Exception e)
                {
                    String message = e.Message;
                }

            });

        }

        private void SetPage(Page page)
        {
            this.frame.Content = page;

        }
        private void ShowLeftBar()
        {
            main.Dispatcher.Invoke(() => {
                LeftBar.Width = 150;
                LeftBar.Visibility = Visibility.Visible;
            });
        }
    }
    public class Window1ViewModel
    {

        public ICommand ButtonCommand
        {
            get
            {
                return new DelegateCommand<string>((str) => {

                    MainWindow.main.leftButtonBarCommand(str);
                });
            }
        }
    }

    public class CookieVisitor : CefSharp.ICookieVisitor
    {
        public event Action<CefSharp.Cookie> SendCookie;

        public void Dispose()
        {
        }

        public bool Visit(CefSharp.Cookie cookie, int count, int total, ref bool deleteCookie)
        {
            deleteCookie = false;
            SendCookie?.Invoke(cookie);

            return true;
        }
    }

}
