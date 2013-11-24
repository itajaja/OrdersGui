using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Hylasoft.OrdersGui.View.Converters
{
    public class EnumToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var par = new StringBuilder((string)parameter);
            var invert = par[0] == '!'; //invert
            var right = invert ? Visibility.Collapsed : Visibility.Visible;
            var wrong = invert ? Visibility.Visible : Visibility.Collapsed;
            try
            {
                var pars = (invert ? par.Remove(0, 1) : par).ToString().Split(new[] { ',' });
                var enumType = value.GetType();
                var ret = pars.Any(s => Enum.Parse(enumType, s, true).Equals(value)) ? right : wrong;
                return ret;
            }
            catch
            {
                return wrong;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}