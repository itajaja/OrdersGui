namespace Hylasoft.OrdersGui.Messages
{
    public class LoadingCompleteMessage
    {

    }

    public class StartLoadingMessage
    {
        public string LoadingMessage { get; set; }
        public bool IsStoppable { get; set; }

        public StartLoadingMessage()
        {
            LoadingMessage = "Loading";
        }

        public StartLoadingMessage(string message,bool isStoppable)
        {
            LoadingMessage = message;
            IsStoppable = isStoppable;
        }
    }
}
