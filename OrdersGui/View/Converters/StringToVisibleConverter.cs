﻿using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Hylasoft.OrdersGui.View.Converters
{
    public class StringToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var par = new StringBuilder(parameter.ToString());
            var invert = par[0] == '!'; //invert
            var right = invert ? Visibility.Collapsed : Visibility.Visible;
            var wrong = invert ? Visibility.Visible : Visibility.Collapsed;
            try
            {
                var pars = (invert ? par.Remove(0, 1) : par).ToString().Split(new[] { ',' });
                var ret = pars.Any(s => s.Equals(value.ToString())) ? right : wrong;
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