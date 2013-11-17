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
            Messenger.Default.Register<GoToLodMessage>(this, o =>
            {
                SlideAnimation.To = -Width;
                ToLod.Begin();
            });
            Messenger.Default.Register<GoToLomMessage>(this, o =>
            {
                ToLom.Begin();
            });
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Width = ActualWidth;
            SizeChanged -= OnSizeChanged;
        }
    }
}
