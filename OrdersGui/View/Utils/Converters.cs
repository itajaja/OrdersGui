using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Hylasoft.OrdersGui.View.Utils
{
    //Generic Converters
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as bool? == true) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
                return (Visibility)value == Visibility.Visible;
            return false;
        }
    }


    //SLOM Specific Converters


}
