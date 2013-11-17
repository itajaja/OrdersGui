using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Hylasoft.OrdersGui.Model;

namespace Hylasoft.OrdersGui.View.Utils
{
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