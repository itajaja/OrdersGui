using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Hylasoft.OrdersGui.Messages;

namespace Hylasoft.OrdersGui.View
{
    /// <summary>
    /// Description for CreateOrder.
    /// </summary>
    public partial class CreateOrder
    {
        /// <summary>
        /// Initializes a new instance of the CreateOrder class.
        /// </summary>
        public CreateOrder()
        {
            InitializeComponent();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new GoToLomMessage());
        }
    }
}