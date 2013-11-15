using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Hylasoft.OrdersGui.Model;

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
    public class OpcConnectionStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (OpcConnectionStatus) value;
            switch (status)
            {
                case OpcConnectionStatus.Running:
                    return new SolidColorBrush(Colors.Green);
                case OpcConnectionStatus.Failed:
                    return new SolidColorBrush(Colors.Brown);                    
                case OpcConnectionStatus.NoConfiguration:
                    return new SolidColorBrush(Colors.DarkGray);
                case OpcConnectionStatus.Suspended:
                    return new SolidColorBrush(Colors.Yellow);
                case OpcConnectionStatus.Test:
                    return new SolidColorBrush(Colors.Blue);
                case OpcConnectionStatus.Disconnected:
                    return new SolidColorBrush(Colors.Brown);
                case OpcConnectionStatus.Unknown:
                    return new SolidColorBrush(Colors.DarkGray);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }

    public class SlomConnectionStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (SlomConnectionStatus)value;
            switch (status)
            {
                case SlomConnectionStatus.Connected:
                    return new SolidColorBrush(Colors.Green);
                case SlomConnectionStatus.Disconnected:
                    return new SolidColorBrush(Colors.Brown);
                case SlomConnectionStatus.Unknown:
                    return new SolidColorBrush(Colors.DarkGray);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }

}
