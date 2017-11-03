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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CallCenterForWpf.Control
{
    /// <summary>
    /// Editex.xaml 的交互逻辑
    /// </summary>
    public partial class Editex : UserControl
    {
        public String Text
        {
            get
            {
                return SourceText.Text;
            }
            set
            {
                SourceText.Text = value;
            }
        }

        public Editex(String text)
        {
            InitializeComponent();
            EditGroup.Visibility = Visibility.Hidden;
            EditGroup.Width = 0;
            Text = text;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditText.Text.Trim() == "")
            {
                MessageBox.Show("编辑内容不能为空！");
                return;
            }
            var objString = EditText.Text;
            EditGroup.Visibility = Visibility.Hidden;
            EditGroup.Width = GetTextBlockWidth(objString);
        }

        double GetTextBlockWidth(String var)
        {
            TextBlock txtBlock = new TextBlock { Text = var };
            txtBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Size sizeText = txtBlock.DesiredSize;
            return sizeText.Width;
        }

        private void SourceText_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SourceText.Visibility = Visibility.Hidden;
            SourceText.Width = 0;
            EditGroup.Visibility = Visibility.Visible;
            String strText = SourceText.Text;
            EditGroup.Width = GetEditGroupWidth(strText);
        }

        double GetEditGroupWidth(String var)
        {
            TextBox txtBox = new TextBox { Text = var };
            txtBox.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Size sizeText = txtBox.DesiredSize;
            return sizeText.Width + OKButton.Width;
        }

        private void EditText_KeyDown(object sender, KeyEventArgs e)
        {
            var str = EditText.Text;
            var width = GetEditGroupWidth(str);
            if(width <= OKButton.Width*2)
            {
                width = OKButton.Width * 2;
            }
            EditGroup.Width = width;
        }
    }
}
