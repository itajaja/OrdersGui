using Hylasoft.OrdersGui.Model;

namespace Hylasoft.OrdersGui.Messages
{
    public class GoToAtMessage
    {
        public bool GoBack { get; set; }

        public GoToAtMessage(bool goBack)
        {
            GoBack = goBack;
        }
    }
}
