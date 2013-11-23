using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Hylasoft.OrdersGui.View.Converters
{
    public class EnumToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var enumType = value.GetType();
                var par = Enum.Parse(enumType, (string)parameter, true);
                return par.Equals(value) ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }

//    public class OrderTypeToVisibleConverter : EnumToVisibleConverter<Model.OrderType> { }
//    public class DetailModeToVisibleConverter : EnumToVisibleConverter<Messages.DetailMode> { }
}