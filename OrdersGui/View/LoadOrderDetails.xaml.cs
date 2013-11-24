using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Hylasoft.OrdersGui.ViewModel;

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
            Messenger.Default.Register<OpenCloseEditDateMessage>(this, message =>
            {
                if (message.Open)
                    OpenEditDate.Begin();
                else
                    CloseEditDate.Begin();
            });
        }
    }
}