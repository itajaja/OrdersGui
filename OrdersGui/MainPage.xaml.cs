using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Hylasoft.OrdersGui.Messages;

namespace Hylasoft.OrdersGui
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            Messenger.Default.Register<GoToLodMessage>(this, _ => ToLod.Begin());
            Messenger.Default.Register<GoToCreateOrderMessage>(this, _ => ToCreate.Begin());
            Messenger.Default.Register<GoToLomMessage>(this, _ => ToLom.Begin());
            Messenger.Default.Register<LoadingCompleteMessage>(this, _ => DispatcherHelper.CheckBeginInvokeOnUI(() => 
            {
                LoadingBusy.IsBusy = false;
                Rootgrid.Visibility = Visibility.Visible;
            }));
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
