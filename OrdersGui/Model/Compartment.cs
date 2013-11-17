using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.Model
{
    public class Compartment : NotifyPropertyChanged
    {
        private int _capacity;
        public int Capacity
        {
            get { return _capacity; }
            set { SetField(ref _capacity, value, "Capacity"); }
        }

        private int _compartmentNo;
        public int CompartmentNo
        {
            get { return _compartmentNo; }
            set { SetField(ref _compartmentNo, value, "CompartmentNo"); }
        }

        private Container _container;
        public Container Container
        {
            get { return _container; }
            set { SetField(ref _container, value, "Container"); }
        }

        #region Equality code

        public override int GetHashCode()
        {
            return (Container.GetHashCode() + CompartmentNo.GetHashCode()).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Compartment)obj);
        }

        public bool Equals(Compartment other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _container == other._container && _compartmentNo == other._compartmentNo;
        }

        public static bool operator ==(Compartment left, Compartment right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(Compartment left, Compartment right)
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
