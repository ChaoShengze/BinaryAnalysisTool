using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace BinaryAnalysisTool
{
    public class ConditionConfigItem : ListBoxItem
    {
        /// <summary>
        /// 配置的类型，由0到2分别为：不启用、按位置、按内容
        /// </summary>
        public int Type;
        /// <summary>
        /// 依据Type进一步的条件，按位置格式“起点索引,终点索引”，按内容则为具体内容
        /// </summary>
        public string Condition;
        /// <summary>
        /// 匹配该条件的内容的显示的颜色
        /// </summary>
        public SolidColorBrush Color;
    }
}
