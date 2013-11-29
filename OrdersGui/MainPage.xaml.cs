using System.Collections.Generic;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Hylasoft.OrdersGui.Messages;

namespace Hylasoft.OrdersGui
{
    public partial class MainPage
    {
        //todo is it needed
        private List<StartLoadingMessage> _loadingStack = new List<StartLoadingMessage>(); 
        
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
            Messenger.Default.Register<StartLoadingMessage>(this, message => DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                LoadingBusy.BusyContent = message.LoadingMessage;
                LoadingBusy.IsBusy = true;
//                CancelLoading.Visibility = message.IsStoppable ? Visibility.Visible : Visibility.Collapsed;
            }));
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Width = ActualWidth;
            SlideAnimation1.To = -ActualWidth;
            SlideAnimation2.To = -ActualWidth;
            SizeChanged -= OnSizeChanged;
        }

        private void CancelLoading_OnClick(object sender, RoutedEventArgs e)
        {
            LoadingBusy.IsBusy = false;
        }
    }
}
