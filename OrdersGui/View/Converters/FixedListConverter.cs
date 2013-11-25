using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Hylasoft.OrdersGui.Model;

namespace Hylasoft.OrdersGui.View.Converters
{
    public class FixedListConverter<T> : IValueConverter where T : class
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sourceList = value != null ? (IEnumerable<T>) value : new List<T>();
            var size = int.Parse((string) parameter);
            var list = new List<T>(sourceList);
            while (list.Count < size)
                list.Add(null);
            return list;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }

    public class OrderProductFixedListConverter : FixedListConverter<OrderProduct>
    {
    }

    public class OrderCompFixedListConverter : FixedListConverter<OrderCompartment>
    {
    }

    public class CompFixedListConverter : FixedListConverter<Compartment>
    {
    }
}