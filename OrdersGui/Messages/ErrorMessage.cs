using System;

namespace Hylasoft.OrdersGui.Messages
{
    public class ErrorMessage
    {
        public string Message { get; private set; }

        public Exception Exception { get; private set; }

        public ErrorMessage(Exception exception, string message)
        {
            Message = message;
            Exception = exception;
        }
    }
}
