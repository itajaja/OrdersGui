﻿using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Messaging;
using Hylasoft.OrdersGui.Messages;

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
            Messenger.Default.Send(new GoToCreateOrderMessage());
        }

        private void OrdersGrid_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var elementsUnderMouse = VisualTreeHelper.FindElementsInHostCoordinates(e.GetPosition(null), this);
            DataGridRow row = elementsUnderMouse
                    .Where(uie => uie is DataGridRow)
                    .Cast<DataGridRow>()
                    .FirstOrDefault();
            if (row != null)
                OrdersGrid.SelectedItem = row.DataContext;
            GridContextMenu.DataContext = OrdersGrid.SelectedItem;
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReportGrid.Opacity >= 1)
                CloseFilter.Begin();
            else
                OpenReports.Begin();
        }
    }
}
