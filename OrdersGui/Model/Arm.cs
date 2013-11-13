using System;
using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.Model
{
    public class Arm : NotifyPropertyChanged, IEquatable<Arm>
    {

        private int _armId;
        public int ArmId
        {
            get { return _armId; }
            set { SetField(ref _armId, value, "ArmId"); }
        }

        private int _armNo;
        public int ArmNo
        {
            get { return _armNo; }
            set { SetField(ref _armNo, value, "ArmNo"); }
        }

        private string _armName;
        public string ArmName
        {
            get { return _armName; }
            set { SetField(ref _armName, value, "ArmName"); }
        }

        private Rack _rack;
        public Rack Rack
        {
            get { return _rack; }
            set { SetField(ref _rack, value, "Rack"); }
        }

        private int _materialFamily; //todo model
        public int MaterialFamily
        {
            get { return _materialFamily; }
            set { SetField(ref _materialFamily, value, "MaterialFamily"); }
        }

        #region Equality code

        public override int GetHashCode()
        {
            return ArmId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Arm)obj);
        }

        public bool Equals(Arm other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _armId == other._armId;
        }

        public static bool operator ==(Arm left, Arm right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(Arm left, Arm right)
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
