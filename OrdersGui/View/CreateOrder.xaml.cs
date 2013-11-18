using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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