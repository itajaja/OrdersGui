using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Hylasoft.OrdersGui.Model;

namespace Hylasoft.OrdersGui.View.Utils
{
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
}