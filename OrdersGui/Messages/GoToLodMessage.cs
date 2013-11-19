namespace Hylasoft.OrdersGui.Messages
{

    /// <summary>
    /// This class represents a Message.
    /// This message states when to change page and go to LoadOrderDetails
    /// </summary>
    public class GoToLodMessage
    {
        /// <summary>
        /// The ID of the order to display
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Sets if editing is enabled
        /// </summary>
        public bool IsReadOnly { get; set; }
    }
}
