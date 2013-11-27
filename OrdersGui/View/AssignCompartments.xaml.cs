using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using Hylasoft.OrdersGui.Model;
using Hylasoft.OrdersGui.ViewModel;

namespace Hylasoft.OrdersGui.View
{
    /// <summary>
    /// Description for AssignCompartments.
    /// </summary>
    public partial class AssignCompartments
    {
        /// <summary>
        /// Initializes a new instance of the AssignCompartments class.
        /// </summary>
        public AssignCompartments()
        {
            InitializeComponent();
            Visibility = Visibility.Visible;
            Visibility = Visibility.Collapsed;            
        }

        private void CompComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            var selected = OrderCompsGrid.SelectedItem as OrderCompartment;
            Messenger.Default.Send(new MoveCompMessage { OrderComp = selected });
            Messenger.Default.Send(new UpdateQtyMessage { OrderComp = selected });
        }

        private void ProductComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            var selected = OrderCompsGrid.SelectedItem as OrderCompartment;
            Messenger.Default.Send(new UpdateQtyMessage { OrderComp = selected });
        }

        private void FrameworkElement_OnUnloaded(object sender, RoutedEventArgs e)
        {
            ((ComboBox)sender).ClearValue(Selector.SelectedItemProperty);
            ((ComboBox)sender).ClearValue(ItemsControl.ItemsSourceProperty);
        }
    }
}