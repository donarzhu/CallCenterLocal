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
    /// CorpusControlItem.xaml 的交互逻辑
    /// </summary>
    public partial class CorpusControlItem : UserControl
    {
        const String CheckHeader = "#{";
        const String CheckTail = "}#";
        List<Object> outtextContrlList = new List<Object>();
        static System.Drawing.Color _lineColor = System.Drawing.Color.DarkGray;
        public SolidColorBrush LineColor { get; set; } = new SolidColorBrush(Color.FromRgb(_lineColor.R, _lineColor.G, _lineColor.B));
        String _sourceText = "";
        public String InputText { get { return inputText.Text; } set { inputText.Text = value; } }
        public String OutputText {
            get
            {
                return GetOutText();
            }
            set
            {
                _sourceText = value.Trim();
                CheckText(value);
                ShowContrl();
            }
        }

        private void ShowContrl()
        {
            foreach(Object obj in  outtextContrlList)
            {
                if(obj is TextBlock)
                    outTextControl.Children.Add((TextBlock)obj);
                if (obj is Editex)
                    outTextControl.Children.Add((Editex)obj);
            }
        }

        bool isError = false;
        private void CheckText(string value)
        {
            if (isError)
                return;
            if (value.Trim().Length <= 0)
                return;
            int checkHeadIndex = value.IndexOf(CheckHeader);
            int checkTailIndex = value.IndexOf(CheckTail);
            if ((checkHeadIndex >= 0 && checkTailIndex >= 0 && checkHeadIndex > checkTailIndex)||
                (checkTailIndex >=0 && checkHeadIndex<0)||
                (checkHeadIndex >=0 && checkHeadIndex<0))
            {
                isError = true;
                outtextContrlList.Clear();
                TextBlock textContrl = new TextBlock
                {
                    Text = _sourceText
                };
                outtextContrlList.Add(textContrl);
                return;
            }
            if(checkHeadIndex > 0)
            {
                String text = value.Substring(0, checkHeadIndex);
                TextBlock textContrl = new TextBlock
                {
                    Text = text
                };
                outtextContrlList.Add(textContrl);
                String ret = value.Substring(checkHeadIndex);
                CheckText(ret);
            }
            else
            {
                String text = value.Substring(CheckHeader.Length, checkTailIndex);
                Editex item = new Editex(text);
                outtextContrlList.Add(item);
                String ret = value.Substring(checkTailIndex + CheckTail.Length);
                CheckText(ret);
            }
            

        }

        private string GetOutText()
        {
            String ret="";
            foreach (Object obj in outtextContrlList)
            {
                if (obj is TextBlock)
                    ret += ((TextBlock)obj).Text;
                if (obj is Editex)
                    ret += ((Editex)obj).Text;
            }
            return ret;
        }

        public CorpusControlItem()
        {
            InitializeComponent();
        }

        private void recordButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PlayVoice_Click(object sender, RoutedEventArgs e)
        {

        }

        private void upload_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
