using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

            rTbx_Main.TextChanged += rTbx_Main_TextChanged;

            Load_LocalConfig();
        }

        private string currText = null;

        /// <summary>
        /// 从配置文件加载之前的配置
        /// </summary>
        private void Load_LocalConfig()
        {
            tbx_head.Text = Properties.Settings.Default.FrameHead;
            tbx_end.Text = Properties.Settings.Default.FrameEnd;

            var typeArr = new string[] { "不启用", "按位置", "按内容" };

            var arr = Properties.Settings.Default.Conditions.Split('|');
            foreach (var config in arr)
            {
                if (config == "")
                    continue;

                var confArr = config.Split('$');
                var conf = new ConditionConfigItem();
                conf.Type = Convert.ToInt32(confArr[0]);
                conf.Condition = confArr[1];
                var colorOrder = Convert.ToInt32(confArr[2]);
                conf.Color = CustomColor.ColorArr[colorOrder];
                conf.Content = $@"{typeArr[conf.Type]},{conf.Condition},{CustomColor.ColorNames[colorOrder]}";
                lsbx_cond.Items.Add(conf);
            }
        }

        /// <summary>
        /// 填充Combx的颜色选项
        /// </summary>
        private void Fill_Combx_Color()
        {
            CustomColor.InitColor();
            for (int i = 0; i < CustomColor.ColorTable.Count; i++)
            {
                var color = CustomColor.ColorTable[i];
                var combxItem = new ComboBoxItem();
                combxItem.Background = color.Color;
                combxItem.Content = color.ColorName;
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
        /// 打开二进制文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_OpenBinary_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "任意文件|*.*";
            ofd.ShowDialog();

            if (ofd.FileName != null && ofd.FileName != "")
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

            if (ofd.FileName != null && ofd.FileName != "")
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
                var arr = Regex.Split(currText.Trim(), tbx_head.Text.Trim(), RegexOptions.IgnoreCase);
                foreach (var str in arr)
                    if (str != "")
                        rTbx_Main.AppendText($@"{tbx_head.Text.Trim()} {str.Trim()}{Environment.NewLine}");
                    else
                        rTbx_Main.AppendText(Environment.NewLine);
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
            if (currText == null)
                return;

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
        /// 添加条件按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_AddCond_Click(object sender, RoutedEventArgs e)
        {
            if (tbx_cond.Text != "" && combx_color.SelectedIndex != -1 && combx_type.SelectedIndex != -1)
            {
                var condItem = new ConditionConfigItem();
                condItem.Content = $@"{combx_type.Text},{combx_color.Text},{tbx_cond.Text}";
                condItem.Type = combx_type.SelectedIndex;
                condItem.Color = CustomColor.ColorTable[combx_color.SelectedIndex].Color;
                condItem.Condition = tbx_cond.Text;
                lsbx_cond.Items.Add(condItem);

                lsbx_cond.SelectedIndex = -1;
                combx_color.SelectedIndex = -1;
                combx_type.SelectedIndex = -1;
                tbx_cond.Text = "";

                UpdateConditionConfig();
            }
        }

        /// <summary>
        /// 移除条件按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_DelCond_Click(object sender, RoutedEventArgs e)
        {
            if (lsbx_cond.SelectedIndex != -1)
            {
                lsbx_cond.Items.Remove(lsbx_cond.SelectedItem);

                lsbx_cond.SelectedIndex = -1;
                combx_color.SelectedIndex = -1;
                combx_type.SelectedIndex = -1;
                tbx_cond.Text = "";

                UpdateConditionConfig();
            }
        }

        /// <summary>
        /// 保存当前的条件列表配置
        /// </summary>
        private void UpdateConditionConfig()
        {
            var str = "";
            foreach (ConditionConfigItem item in lsbx_cond.Items)
                str += $@"{item.Type}${item.Condition}${CustomColor.ColorArr.IndexOf(item.Color)}|";

            Properties.Settings.Default.Conditions = str;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// 按照当前设定给Rtbx中的文本上色
        /// </summary>
        private void Handle_Rtbx()
        {
            foreach (ConditionConfigItem item in lsbx_cond.Items)
            {
                switch (item.Type)
                {
                    case 0:
                        break;
                    case 1: //按位置
                        DyeByPostion(item.Condition, item.Color);
                        break;
                    case 2: //按内容
                        DyeByText(item.Condition, item.Color);
                        break;
                }
            }
        }

        /// <summary>
        /// 将指定文本染色
        /// </summary>
        /// <param name="text">匹配文本</param>
        /// <param name="color">指定颜色</param>
        private void DyeByText(string text, SolidColorBrush color)
        {
            if (text == "")
                return;

            Regex r = new Regex(text, RegexOptions.IgnoreCase);
            foreach (var block in rTbx_Main.Document.Blocks.ToArray())
            {
                TextRange all = new TextRange(block.ContentStart, block.ContentEnd);
                foreach (Match m in r.Matches(all.Text))
                {
                    all.Select(
                        block.ContentStart.GetPositionAtOffset(m.Index),
                        block.ContentStart.GetPositionAtOffset(m.Index + m.Length + 1));
                    all.ApplyPropertyValue(TextElement.ForegroundProperty, color);
                }
            }
        }

        /// <summary>
        /// 将指定范围内容染色
        /// </summary>
        /// <param name="postionStr"></param>
        /// <param name="color"></param>
        private void DyeByPostion(string postionStr, SolidColorBrush color)
        {
            if (postionStr.IndexOf(",") == -1)
                return;

            var posArr = postionStr.Split(',');
            foreach (var block in rTbx_Main.Document.Blocks.ToArray())
            {
                TextRange all = new TextRange(block.ContentStart, block.ContentEnd);

                if (all.Text == "")
                    continue;

                all.Select(
                    block.ContentStart.GetPositionAtOffset(Convert.ToInt32(posArr[0]) * 3),
                    block.ContentStart.GetPositionAtOffset(Convert.ToInt32(posArr[1]) * 3));
                all.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            }
        }

        /// <summary>
        /// 文本内容发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rTbx_Main_TextChanged(object sender, TextChangedEventArgs e)
        {
            DyeByText(tbx_head.Text, Brushes.OrangeRed);
            DyeByText(tbx_end.Text, Brushes.BlueViolet);
            Handle_Rtbx();
        }

        /// <summary>
        /// 刷新按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Refresh_Click(object sender, RoutedEventArgs e)
        {
            Clear_Rtbx();
            Handle_CurrText();
            Fill_CurrText();

            rTbx_Main_TextChanged(null, null);
        }

        /// <summary>
        /// 自动保存帧头设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbx_head_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.FrameHead = tbx_head.Text;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// 自动保存帧尾设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbx_end_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.FrameEnd = tbx_end.Text;
            Properties.Settings.Default.Save();
        }
    }
}
