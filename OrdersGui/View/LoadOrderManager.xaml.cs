﻿using System.Windows;
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

        private void LoadOrderManager_OnLoaded(object sender, RoutedEventArgs e)
        {
            OrdersGrid.Columns[0].Header = Strings_en_us.OrderNumberHeader;
            OrdersGrid.Columns[1].Header = Strings_en_us.RackHeader;
            OrdersGrid.Columns[2].Header = Strings_en_us.CustomerHeader;
            OrdersGrid.Columns[3].Header = Strings_en_us.CarrierHeader;
            OrdersGrid.Columns[4].Header = Strings_en_us.DateHeader;
            OrdersGrid.Columns[5].Header = Strings_en_us.TimeHeader;
            OrdersGrid.Columns[6].Header = Strings_en_us.StartTimeHeader;
            OrdersGrid.Columns[7].Header = Strings_en_us.EndTimeHeader;
            OrdersGrid.Columns[8].Header = Strings_en_us.OrderTypeHeader;
            OrdersGrid.Columns[9].Header = Strings_en_us.DeliveryMethodHeader;
            OrdersGrid.Columns[10].Header = Strings_en_us.ShipRefHeader;
            OrdersGrid.Columns[11].Header = Strings_en_us.OrderStatusHeader;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
