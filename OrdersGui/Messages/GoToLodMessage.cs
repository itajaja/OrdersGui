using Hylasoft.OrdersGui.Model;

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
        public Order Order { get; set; }

        /// <summary>
        /// Sets if editing is enabled
        /// </summary>
        public DetailMode Mode { get; set; }    
    }

    public enum DetailMode
    {
        View,
        Edit,
        Prepare,
        Fullfill
    }
}
