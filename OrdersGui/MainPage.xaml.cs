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
//                ToLod.Begin();
                LoadOrderDetailsView.Visibility = Visibility.Visible;
                LoadOrderManagerView.Visibility = Visibility.Collapsed;
            });
            Messenger.Default.Register<GoToLomMessage>(this, o =>
            {
//                ToLom.Begin();
                LoadOrderDetailsView.Visibility = Visibility.Collapsed;
                LoadOrderManagerView.Visibility = Visibility.Visible;
            });
        }
    }
}
