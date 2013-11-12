namespace Hylasoft.OrdersGui.Model
{
    public class SessionData : NotifyPropertyChanged
    {
        private ConnectionStatus _opcStatus;
        public ConnectionStatus OpcStatus
        {
            get { return _opcStatus; }
            set { SetField(ref _opcStatus, value, "OpcStatus"); }
        }

        private ConnectionStatus _slomStatus;
        public ConnectionStatus SlomStatus
        {
            get { return _slomStatus; }
            set { SetField(ref _slomStatus, value, "SlomStatus"); }
        }
    }

    public enum ConnectionStatus
    {
        Connected,
        Disconnected,
        Unknown
    }
}
