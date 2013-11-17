using System;
using System.Globalization;
using System.Windows.Data;
using Hylasoft.OrdersGui.Model;

namespace Hylasoft.OrdersGui.View.Utils
{
    public class OrderStatusToStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string statusString;
            switch ((OrderStatus)value)
            {
                case OrderStatus.Idle:
                    statusString = "IDLE";
                    break;
                case OrderStatus.Ready:
                    statusString = "READY";
                    break;
                case OrderStatus.TruckArrived:
                    statusString = "TRUCK ARRIVED";
                    break;
                case OrderStatus.ReadyForRelease:
                    statusString = "READY FOR RELEASE";
                    break;
                case OrderStatus.ReleaseInProgress:
                    statusString = "RELEASE IN PROGRESS";
                    break;
                case OrderStatus.Released:
                    statusString = "RELEASED";
                    break;
                case OrderStatus.ReleaseError:
                    statusString = "RELEASE ERROR";
                    break;
                case OrderStatus.Running:
                    statusString = "RUNNING";
                    break;
                case OrderStatus.Stopped:
                    statusString = "STOPPED";
                    break;
                case OrderStatus.Completed:
                    statusString = "COMPLETED";
                    break;
                case OrderStatus.Aborted:
                    statusString = "ABORTED";
                    break;
                case OrderStatus.Cancelled:
                    statusString = "CANCELLED";
                    break;
                case OrderStatus.Suspended:
                    statusString = "SUSPENDED";
                    break;
                case OrderStatus.Approved:
                    statusString = "APPROVED";
                    break;
                case OrderStatus.Rejected:
                    statusString = "REJECTED";
                    break;
                case OrderStatus.WaitingForSeals:
                    statusString = "WAITING FOR SEALS";
                    break;
                case OrderStatus.EndFilling:
                    statusString = "END FILLING";
                    break;
                case OrderStatus.ReportReady:
                    statusString = "REPORT READY";
                    break;
                case OrderStatus.ReportRetry:
                    statusString = "REPORT RETRY";
                    break;
                case OrderStatus.UpdateDone:
                    statusString = "UPDATE DONE";
                    break;
                case OrderStatus.Setup:
                    statusString = "SETUP";
                    break;
                case OrderStatus.InspectionFailed:
                    statusString = "INSPECTION FAILED";
                    break;
                case OrderStatus.PendingApproval:
                    statusString = "PENDING APPROVAL";
                    break;
                case OrderStatus.SapUpdateDone:
                    statusString = "SAP UPDATE DONE";
                    break;
                default:
                    statusString = "UNKNOWN";
                    break;
            }
            return statusString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}