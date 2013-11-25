namespace Hylasoft.OrdersGui.Messages
{
    public class GoToAcMessage
    {
        public bool GoBack { get; set; }

        public GoToAcMessage(bool goBack)
        {
            GoBack = goBack;
        }
    }
}
