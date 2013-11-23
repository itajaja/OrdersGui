using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Hylasoft.OrdersGui.Model;

namespace Hylasoft.OrdersGui.View.Converters
{
    public class EnumToCollapsedConverter<T> : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var enumType = typeof(T);
                var par = (T)Enum.Parse(enumType, (string)parameter, true);
                var val = (T)value;
                return par.Equals(val) ? Visibility.Collapsed : Visibility.Visible;
            }
            catch
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }

    public class OrderTypeToCollapsedConverter : EnumToCollapsedConverter<OrderType> { }
}