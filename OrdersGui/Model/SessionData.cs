using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.Model
{
    public class SessionData : NotifyPropertyChanged
    {
        private OpcConnectionStatus _opcStatus;
        public OpcConnectionStatus OpcStatus
        {
            get { return _opcStatus; }
            set { SetField(ref _opcStatus, value, "OpcStatus"); }
        }

        private SlomConnectionStatus _slomStatus;
        public SlomConnectionStatus SlomStatus
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

        private string _ntfConnectionString;
        public string NtfConnectionString
        {
            get { return _ntfConnectionString; }
            set { SetField(ref _ntfConnectionString, value, "NtfConnectionString"); }
        }

        private string _tfConnectionString;
        public string TfConnectionString
        {
            get { return _tfConnectionString; }
            set { SetField(ref _tfConnectionString, value, "TfConnectionString"); }
        }

        private string _emConnectionString;
        public string EmConnectionString
        {
            get { return _emConnectionString; }
            set { SetField(ref _emConnectionString, value, "EmConnectionString"); }
        }
    }

    public enum User
    {
        User0, //only views reports
        User1, //views reports + creates order
        User2 //no limits
    }

    public enum SlomConnectionStatus
    {
        Connected,
        Disconnected,
        Unknown
    }

    public enum OpcConnectionStatus
    {
        Running,
        Failed,
        NoConfiguration,
        Suspended,
        Test,
        Disconnected,
        Unknown
    }
}
