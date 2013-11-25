using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Hylasoft.OrdersGui.Messages;
using Hylasoft.OrdersGui.ViewModel;

namespace Hylasoft.OrdersGui.View
{
    /// <summary>
    /// Description for LoadOrderDetails.
    /// </summary>
    public partial class LoadOrderDetails
    {
        /// <summary>
        /// Initializes a new instance of the LoadOrderDetails class.
        /// </summary>
        public LoadOrderDetails()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenCloseEditDateMessage>(this, message =>
            {
                if (message.Open)
                    OpenEditDate.Begin();
                else
                    CloseEditDate.Begin();
            });
            Messenger.Default.Register<GoToAtMessage>(this, m =>
            {
                if (m.GoBack)
                    GoBack.Begin();
                else
                    ToAt.Begin();
            });
            Messenger.Default.Register<GoToAcMessage>(this, m =>
            {
                if (m.GoBack)
                    GoBack.Begin();
                else
                    ToAc.Begin();
            });
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Width = ActualWidth;
            SlideAnimation1.To = -ActualWidth;
            SlideAnimation2.To = -ActualWidth;
            SizeChanged -= OnSizeChanged;
        }
    }
}