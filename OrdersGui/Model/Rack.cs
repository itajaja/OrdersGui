namespace Hylasoft.OrdersGui.Model
{
    public class Rack : NotifyPropertyChanged
    {
        private int _rackId;
        public int RackId
        {
            get { return _rackId; }
            set { SetField(ref _rackId, value, "RackId"); }
        }

        private string _rackName;
        public string RackName
        {
            get { return _rackName; }
            set { SetField(ref _rackName, value, "RackName"); }
        }

        private int _rackStatus;
        public int RackStatus
        {
            get { return _rackStatus; }
            set { SetField(ref _rackStatus, value, "RackStatus"); }
        }
    }
}
