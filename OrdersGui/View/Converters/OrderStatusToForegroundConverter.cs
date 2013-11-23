using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Hylasoft.OrdersGui.Model;

namespace Hylasoft.OrdersGui.View.Converters
{
    public class OrderStatusToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (((OrderStatus)value))
            {
                case OrderStatus.Idle: return new SolidColorBrush(Colors.Black);
                case OrderStatus.Ready: return new SolidColorBrush(Colors.Black);
                case OrderStatus.TruckArrived: return new SolidColorBrush(Colors.Black);
                case OrderStatus.ReadyForRelease: return new SolidColorBrush(Colors.Black);
                case OrderStatus.ReleaseInProgress: return new SolidColorBrush(Colors.Black);
                case OrderStatus.Released: return new SolidColorBrush(Colors.Black);
                case OrderStatus.ReleaseError: return new SolidColorBrush(Colors.Black);

                case OrderStatus.Running: return new SolidColorBrush(Colors.Black);
                case OrderStatus.Stopped: return new SolidColorBrush(Colors.Black);
                case OrderStatus.Completed: return new SolidColorBrush(Colors.Black);
                case OrderStatus.Aborted: return new SolidColorBrush(Colors.Black);
                case OrderStatus.Cancelled: return new SolidColorBrush(Colors.Black);
                case OrderStatus.Suspended: return new SolidColorBrush(Colors.Black);
                case OrderStatus.Approved: return new SolidColorBrush(Colors.Black);
                case OrderStatus.Rejected: return new SolidColorBrush(Colors.Black);
                case OrderStatus.WaitingForSeals: return new SolidColorBrush(Colors.Black);
                case OrderStatus.EndFilling: return new SolidColorBrush(Colors.Black);
                case OrderStatus.ReportReady: return new SolidColorBrush(Colors.White);
                case OrderStatus.ReportRetry: return new SolidColorBrush(Colors.White);
                case OrderStatus.UpdateDone: return new SolidColorBrush(Colors.White);
                case OrderStatus.PendingApproval: return new SolidColorBrush(Colors.Black);
                case OrderStatus.Setup: return new SolidColorBrush(Colors.White);
                case OrderStatus.InspectionFailed: return new SolidColorBrush(Colors.Black);
                case OrderStatus.SapUpdateDone: return new SolidColorBrush(Colors.Black);
                default: return new SolidColorBrush(Colors.Black);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}