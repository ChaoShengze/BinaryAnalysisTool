using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace BinaryAnalysisTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Fill_Combx_Color();
        }

        private string currText = null;


        private void Fill_Combx_Color()
        {
            var colorStr = "255,0,0|65,105,225|0,206,209|107,142,45|0,0,128|60,179,113|255,228,196|176,196,222|255,160,122|255,255,0";
            var colorName = "Red|RoyalBlue|DarkTurquoise|OliveDrab|Navy|MediumSeaGreen|Bisque|LightSteelBlue|LightSalmon|Yellow";
            var colorArr = colorStr.Split('|');
            var nameArr = colorName.Split('|');
            for (int i = 0; i < colorArr.Length; i++)
            {
                var singleColorArr = colorArr[i].Split(',');
                var combxItem = new ComboBoxItem();
                combxItem.Background = new SolidColorBrush(Color.FromRgb(
                    Convert.ToByte(singleColorArr[0]), Convert.ToByte(singleColorArr[1]), Convert.ToByte(singleColorArr[2])));
                combxItem.Content = nameArr[i];
                combx_color.Items.Add(combxItem);
            }
        }

        /// <summary>
        /// 退出按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 加载文本文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_OpenText_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "文本文件|*.txt";
            ofd.ShowDialog();

            if (ofd.FileName != null)
                currText = File.ReadAllText(ofd.FileName);

            Clear_Rtbx();
            Handle_CurrText();
            Fill_CurrText();
        }

        /// <summary>
        /// 将CurrText填充到Rtbx中
        /// </summary>
        private void Fill_CurrText()
        {
            if (tbx_head.Text != "")
            {
                // 如果设置了帧头帧尾，则自动添加换行
                var arr = currText.Split(new string[] { tbx_head.Text }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var str in arr)
                    rTbx_Main.AppendText($@"{tbx_head.Text}{str}{Environment.NewLine}");
            }
            else
            {
                rTbx_Main.AppendText(currText);
            }
        }

        /// <summary>
        /// 处理当前读取的文本
        /// </summary>
        private void Handle_CurrText()
        {
            // 格式化文本，按两位显示
            currText.Replace("0x", "");
            currText.Replace("0X", "");
            currText.Trim();
        }

        /// <summary>
        /// 清空Rtbx
        /// </summary>
        private void Clear_Rtbx()
        {
            rTbx_Main.Document.Blocks.Clear();
        }

        /// <summary>
        /// 按照当前设定给Rtbx中的文本上色
        /// </summary>
        private void Handle_Rtbx()
        {

        }

        /// <summary>
        /// 打开二进制文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_OpenBinary_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "文本文件|*.txt";
            ofd.ShowDialog();

            if (ofd.FileName != null)
            {
                var arr = File.ReadAllBytes(ofd.FileName);
                var txt = "";
                foreach (var bin in arr)
                    txt += bin.ToString("X2") + " ";

                currText = txt;

                Clear_Rtbx();
                Handle_CurrText();
                Fill_CurrText();
            }
        }
    }
}
