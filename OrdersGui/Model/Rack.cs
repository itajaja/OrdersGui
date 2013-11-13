using Hylasoft.OrdersGui.Model.Service;

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

        #region Equality code

        public override int GetHashCode()
        {
            return RackId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Rack)obj);
        }

        public bool Equals(Rack other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _rackId == other._rackId;
        }

        public static bool operator ==(Rack left, Rack right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(Rack left, Rack right)
        {
            if (ReferenceEquals(left, right))
                return false;
            if (ReferenceEquals(left, null))
                return true;
            return !left.Equals(right);
        }

        #endregion
    }
}
