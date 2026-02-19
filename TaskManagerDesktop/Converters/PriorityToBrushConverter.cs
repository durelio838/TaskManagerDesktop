using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TaskManagerDesktop.Models;

namespace TaskManagerDesktop.Converters
{
    public class PriorityToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Priority priority)
            {
                return priority.Color switch
                {
                    "#D32F2F" => new SolidColorBrush(Color.FromRgb(211, 47, 47)),
                    "#F57C00" => new SolidColorBrush(Color.FromRgb(245, 124, 0)),
                    "#FBC02D" => new SolidColorBrush(Color.FromRgb(251, 192, 45)),
                    "#388E3C" => new SolidColorBrush(Color.FromRgb(56, 142, 60)),
                    _ => new SolidColorBrush(Colors.Gray)
                };
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
