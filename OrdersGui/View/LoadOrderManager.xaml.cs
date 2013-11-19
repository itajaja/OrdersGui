using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Hylasoft.OrdersGui.Messages;
using Hylasoft.OrdersGui.Resources;

namespace Hylasoft.OrdersGui.View
{
    /// <summary>
    /// Description for LoadOrderManager.
    /// </summary>
    public partial class LoadOrderManager
    {
        /// <summary>
        /// Initializes a new instance of the LoadOrderManager class.
        /// </summary>
        public LoadOrderManager()
        {
            InitializeComponent();
        }
    
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            //todo this should be command
            Messenger.Default.Send(new GoToLodMessage{OrderId = 0}); //todo we don't like order id here, of course!
        }

        private void OrderStatusFilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrderStatusFilterGrid.Opacity >= 1)
                CloseFilter.Begin();
            else
                OpenStatusFilter.Begin();
        }

        private void DateFilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (DateFilterGrid.Opacity >= 1)
                CloseFilter.Begin();
            else
                OpenDateFilter.Begin();
        }

        private void CollapseFilters_Click(object sender, RoutedEventArgs e)
        {
            CloseFilter.Begin();
        }

        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new GoToCreateOrderMessage()); //todo this should be command
        }
    }
}
