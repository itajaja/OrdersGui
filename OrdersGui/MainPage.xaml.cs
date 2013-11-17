using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Hylasoft.OrdersGui.Messages;

namespace Hylasoft.OrdersGui
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            Messenger.Default.Register<GoToLodMessage>(this, o => ToLod.Begin());
            Messenger.Default.Register<GoToCreateOrderMessage>(this, o => ToCreate.Begin());
            Messenger.Default.Register<GoToLomMessage>(this, o => ToLom.Begin());
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
