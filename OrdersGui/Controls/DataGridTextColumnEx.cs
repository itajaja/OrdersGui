using System.Windows;
using System.Windows.Controls;

namespace Hylasoft.OrdersGui.Controls
{
    public class DataGridTextColumnEx : DataGridTextColumn
    {

        public static readonly DependencyProperty
            VisibilityDependencyProperty = DependencyProperty.Register(
                "DVisibility",
                typeof(Visibility),
                typeof(DataGridTextColumnEx),
                new PropertyMetadata(OnDVisibilityChanged));

        public Visibility DVisibility
        {
            get { return (Visibility)GetValue(VisibilityDependencyProperty); }
            set { SetValue(VisibilityDependencyProperty, value); }
        }

        private static void OnDVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DataGridTextColumnEx;
            var b = (Visibility)e.NewValue;
            control.Visibility = b;
        }
    }
}