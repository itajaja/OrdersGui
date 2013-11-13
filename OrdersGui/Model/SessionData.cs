using Hylasoft.OrdersGui.Model.Service;

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

        private User _user;
        public User User
        {
            get { return _user; }
            set { SetField(ref _user, value, "User"); }
        }

        private string _connectionString;
        public string ConnectionString
        {
            get { return _connectionString; }
            set { SetField(ref _connectionString, value, "ConnectionString"); }
        }
    }

    public enum User
    {
        User0, //only views reports
        User1, //views reports + creates order
        User2 //no limits
    }

    public enum ConnectionStatus
    {
        Connected,
        Disconnected,
        Unknown
    }
}
