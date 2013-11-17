using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.Model
{
    public class Container : NotifyPropertyChanged
    {
        private int _capacity;
        public int Capacity
        {
            get { return _capacity; }
            set { SetField(ref _capacity, value, "Capacity"); }
        }


        private int _compartmentsCount;
        public int CompartmentsCount
        {
            get { return _compartmentsCount; }
            set { SetField(ref _compartmentsCount, value, "CompartmentsCount"); }
        }


        private long _containerId;
        public long ContainerId
        {
            get { return _containerId; }
            set { SetField(ref _containerId, value, "ContainerId"); }
        }


        private string _containerNo;
        public string ContainerNo
        {
            get { return _containerNo; }
            set { SetField(ref _containerNo, value, "ContainerNo"); }
        }


        private double _grossWeight;
        public double GrossWeight
        {
            get { return _grossWeight; }
            set { SetField(ref _grossWeight, value, "GrossWeight"); }
        }


        private int _type;
        public int Type
        {
            get { return _type; }
            set { SetField(ref _type, value, "Type"); }
        }

        #region Equality code

        public override int GetHashCode()
        {
            return ContainerId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Container)obj);
        }

        public bool Equals(Container other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _containerId == other.ContainerId;
        }

        public static bool operator ==(Container left, Container right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(Container left, Container right)
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
