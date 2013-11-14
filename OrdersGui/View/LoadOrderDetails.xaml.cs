using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Hylasoft.OrdersGui.Messages;

namespace Hylasoft.OrdersGui.View
{
    /// <summary>
    /// Description for LoadOrderDetails.
    /// </summary>
    public partial class LoadOrderDetails
    {
        /// <summary>
        /// Initializes a new instance of the LoadOrderDetails class.
        /// </summary>
        public LoadOrderDetails()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new GoToLomMessage());
        }
    }
}