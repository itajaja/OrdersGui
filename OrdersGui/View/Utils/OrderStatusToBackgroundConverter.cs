using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Hylasoft.OrdersGui.Model;

namespace Hylasoft.OrdersGui.View.Utils
{
    public class OrderStatusToBackgroundConverter : IValueConverter
    {
        private static readonly Color LightGreen = Color.FromArgb(255, 123, 252, 49);
        private static readonly Color Pink = Color.FromArgb(255, 255, 0, 222);
        private static readonly Color DeepPink = Color.FromArgb(255, 255, 92, 205);
        private static readonly Color LightPurple = Color.FromArgb(255, 179, 0, 185);
        private static readonly Color AquaMarineBlue = Color.FromArgb(255, 27, 161, 200);
        private static readonly Color DarkBlue = Color.FromArgb(255, 28, 3, 255);
        private static readonly Color Turquoise = Color.FromArgb(255, 48, 213, 200);
        private static readonly Color BrightCerulean = Color.FromArgb(255, 2, 164, 211);
        private static readonly Color Peru = Color.FromArgb(255, 205, 133, 63);
        private static readonly Color StatusIdle = Colors.Transparent;
        private static readonly Color StatusReady = Colors.Transparent;
        private static readonly Color StatusTruckArrived = Colors.Transparent;
        private static readonly Color StatusReadyForRel = Colors.Cyan;
        private static readonly Color StatusRelInProgress = Turquoise;
        private static readonly Color StatusReleased = BrightCerulean;
        private static readonly Color StatusReleasedError = Colors.Red;
        private static readonly Color StatusRunning = LightGreen;
        private static readonly Color StatusStopped = Colors.Yellow;
        private static readonly Color StatusCompleted = Colors.Green;
        private static readonly Color StatusAborted = DeepPink;
        private static readonly Color StatusCancelled = DeepPink;
        private static readonly Color StatusSuspended = Colors.Yellow;
        private static readonly Color StatusApproved = Colors.Green;
        private static readonly Color StatusRejected = Colors.Transparent;
        private static readonly Color StatusWaitingForSeals = Colors.Orange;
        private static readonly Color StatusEndFilling = Colors.Green;
        private static readonly Color StatusReportReady = DarkBlue;
        private static readonly Color StatusReportRetry = DarkBlue;
        private static readonly Color StatusUpdateDone = DarkBlue;
        private static readonly Color StatusSetup = DarkBlue;
        private static readonly Color StatusPendingApproval = Colors.Orange;
        private static readonly Color StatusInspectionFailed = Colors.Red;
        private static readonly Color StatusSapUpdateDone = Colors.Brown;
        // ReSharper restore InconsistentNaming
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (((OrderStatus)value))
            {
                case OrderStatus.Idle: return new SolidColorBrush(StatusIdle);
                case OrderStatus.Ready: return new SolidColorBrush(StatusReady);
                case OrderStatus.TruckArrived: return new SolidColorBrush(StatusTruckArrived);
                case OrderStatus.ReadyForRelease: return new SolidColorBrush(StatusReadyForRel);
                case OrderStatus.ReleaseInProgress: return new SolidColorBrush(StatusRelInProgress);
                case OrderStatus.Released: return new SolidColorBrush(StatusReleased);
                case OrderStatus.ReleaseError: return new SolidColorBrush(StatusReleasedError);
                case OrderStatus.Running: return new SolidColorBrush(StatusRunning);
                case OrderStatus.Stopped: return new SolidColorBrush(StatusStopped);
                case OrderStatus.Completed: return new SolidColorBrush(StatusCompleted);
                case OrderStatus.Aborted: return new SolidColorBrush(StatusAborted);
                case OrderStatus.Cancelled: return new SolidColorBrush(StatusCancelled);
                case OrderStatus.Suspended: return new SolidColorBrush(StatusSuspended);
                case OrderStatus.Approved: return new SolidColorBrush(StatusApproved);
                case OrderStatus.Rejected: return new SolidColorBrush(StatusRejected);
                case OrderStatus.WaitingForSeals: return new SolidColorBrush(StatusWaitingForSeals);
                case OrderStatus.EndFilling: return new SolidColorBrush(StatusEndFilling);
                case OrderStatus.ReportReady: return new SolidColorBrush(StatusReportReady);
                case OrderStatus.ReportRetry: return new SolidColorBrush(StatusReportRetry);
                case OrderStatus.UpdateDone: return new SolidColorBrush(StatusUpdateDone);
                case OrderStatus.PendingApproval: return new SolidColorBrush(StatusPendingApproval);
                case OrderStatus.Setup: return new SolidColorBrush(StatusSetup);
                case OrderStatus.InspectionFailed: return new SolidColorBrush(StatusInspectionFailed);
                case OrderStatus.SapUpdateDone: return new SolidColorBrush(Peru);
                default: return new SolidColorBrush(Colors.Transparent);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}