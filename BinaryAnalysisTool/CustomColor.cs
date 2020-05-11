using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BinaryAnalysisTool
{
    class CustomColor
    {
        public string ColorName;
        public SolidColorBrush Color;

        public static List<CustomColor> ColorTable = new List<CustomColor>();
        public static List<SolidColorBrush> ColorArr = new List<SolidColorBrush>()
        {
            Brushes.Red, Brushes.Blue, Brushes.Green, Brushes.Yellow, Brushes.Orange,
            Brushes.Brown, Brushes.Gray, Brushes.Violet, Brushes.Salmon, Brushes.Cyan
        };
        public static List<string> ColorNames = new List<string>() 
        { 
            "Red", "Blue", "Green", "Yellow", "Orange",
            "Brown", "Gray", "Violet", "Salmon", "Cyan"
        };

        public static void InitColor()
        {
            for (int i = 0; i < ColorArr.Count; i++)
            {
                var color = new CustomColor();
                color.Color = ColorArr[i];
                color.ColorName = ColorNames[i];
                ColorTable.Add(color);
            }
        }
    }
}
