using System;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Hylasoft.OrdersGui.Messages;
using Hylasoft.OrdersGui.NonTransactionalFunctions;

namespace Hylasoft.OrdersGui
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            Messenger.Default.Register<GoToLodMessage>(this, o =>
            {
                LoadOrderDetailsView.Visibility = Visibility.Visible;
                LoadOrderManagerView.Visibility = Visibility.Collapsed;
            });
            Messenger.Default.Register<GoToLomMessage>(this, o =>
            {
                LoadOrderDetailsView.Visibility = Visibility.Collapsed;
                LoadOrderManagerView.Visibility = Visibility.Visible;
            });
        }
    }
}
