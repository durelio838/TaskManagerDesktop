using System;
using System.Globalization;
using System.Windows.Data;
using TaskManagerDesktop.Models;

namespace TaskManagerDesktop.Converters
{
    public class PriorityDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Priority priority)
                return priority.Name ?? "-";
            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
