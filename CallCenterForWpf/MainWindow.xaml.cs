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

namespace CallCenterForWpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        double SrcWidth { get; set; }
        double SrcHeight { get; set; }
        Boolean IsMax { get; set; } = false;
        DateTime testHitTime = DateTime.Now;
        DateTime AppStartTime { get; } = DateTime.Now;
        public static MainWindow main;
        public MainWindow()
        {
            InitializeComponent();
            main = this;
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

            //调整窗口最大化,全屏的关键代码就是下面3句 
            this.MaxWidth = screen.Bounds.Width;
            this.MaxHeight = screen.Bounds.Height;
            this.WindowState = WindowState.Maximized;

            IsMax = true;
            testHitTime = AppStartTime;
        }

        private void ExitMaxWindow()
        {
            if (!IsMax)
                return;
            this.MaxWidth = SrcWidth;
            this.MaxHeight = SrcHeight;
            this.WindowState = WindowState.Normal;
            IsMax = false;
            testHitTime = AppStartTime;
        }

        public void leftButtonBarCommand(string param)
        {
            System.Windows.MessageBox.Show("Button's parameter:" + param);
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            var span = DateTime.Now - testHitTime;
            if(span.Milliseconds <500)
            {
                if (IsMax)
                    ExitMaxWindow();
                else
                    WindowMax();

            }

            testHitTime = DateTime.Now;
        }


        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(sender == closeButton)
                System.Windows.Application.Current.Shutdown(-1);
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
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
}
