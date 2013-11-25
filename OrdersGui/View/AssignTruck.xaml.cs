using System.Windows;
using System.Windows.Controls;

namespace Hylasoft.OrdersGui.View
{
    /// <summary>
    /// Description for AssignTruck.
    /// </summary>
    public partial class AssignTruck
    {
        /// <summary>
        /// Initializes a new instance of the AssignTruck class.
        /// </summary>
        public AssignTruck()
        {
            InitializeComponent();
            Visibility = Visibility.Visible;
            Visibility = Visibility.Collapsed;
        }

        private void UpdateTruck(object sender, RoutedEventArgs e)
        {
            var be = TruckGrid.GetBindingExpression(DataGrid.SelectedItemProperty);
            be.UpdateSource();
        }
    }
}