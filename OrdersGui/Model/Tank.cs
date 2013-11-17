using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.Model
{
    public class Tank : NotifyPropertyChanged
    {
        private double _apiGravity;
        public double ApiGravity
        {
            get { return _apiGravity; }
            set { SetField(ref _apiGravity, value, "ApiGravity"); }
        }

        private bool _availabilityStatus;
        public bool AvailabilityStatus
        {
            get { return _availabilityStatus; }
            set { SetField(ref _availabilityStatus, value, "AvailabilityStatus"); }
        }

        private Material _material;
        public Material Material
        {
            get { return _material; }
            set { SetField(ref _material, value, "Material"); }
        }

        private string _sapTankName;
        public string SapTankName
        {
            get { return _sapTankName; }
            set { SetField(ref _sapTankName, value, "SapTankName"); }
        }

        private int _tankId;
        public int TankId
        {
            get { return _tankId; }
            set { SetField(ref _tankId, value, "TankId"); }
        }

        private string _tankName;
        public string TankName
        {
            get { return _tankName; }
            set { SetField(ref _tankName, value, "TankName"); }
        }

        #region Equality code

        public override int GetHashCode()
        {
            return TankId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Tank)obj);
        }

        public bool Equals(Tank other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _tankId == other._tankId;
        }

        public static bool operator ==(Tank left, Tank right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(Tank left, Tank right)
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
