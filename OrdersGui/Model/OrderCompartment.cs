using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.Model
{
    public class OrderCompartment : NotifyPropertyChanged
    {

        private LoadingCompartmentStatus _compartmentStatus;
        public LoadingCompartmentStatus CompartmentStatus
        {
            get { return _compartmentStatus; }
            set { SetField(ref _compartmentStatus, value, "CompartmentStatus"); }
        }

        private Compartment _compartment;
        public Compartment Compartment
        {
            get { return _compartment; }
            set { SetField(ref _compartment, value, "Compartment"); }
        }

        private OrderProduct _orderProduct;
        public OrderProduct OrderProduct
        {
            get { return _orderProduct; }
            set { SetField(ref _orderProduct, value, "OrderProduct"); }
        }

        private Arm _rackArm;
        public Arm RackArm
        {
            get { return _rackArm; }
            set { SetField(ref _rackArm, value, "RackArm"); }
        }

        private double _actualQty;
        public double ActualQty
        {
            get { return _actualQty; }
            set { SetField(ref _actualQty, value, "ActualQty"); }
        }

        private double _targetQty;
        public double TargetQty
        {
            get { return _targetQty; }
            set { SetField(ref _targetQty, value, "TargetQty"); }
        }

        private int _seqNo;
        public int SeqNo
        {
            get { return _seqNo; }
            set { SetField(ref _seqNo, value, "SeqNo"); }
        }

        private double _netWeight;
        public double NetWeight
        {
            get { return _netWeight; }
            set { SetField(ref _netWeight, value, "NetWeight"); }
        }

        private Tank _tank;
        public Tank Tank
        {
            get { return _tank; }
            set { SetField(ref _tank, value, "Tank"); }
        }

        #region Equality code

        public override int GetHashCode()
        {
            return (OrderProduct.GetHashCode()+SeqNo.GetHashCode()).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OrderCompartment)obj);
        }

        public bool Equals(OrderCompartment other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return (_orderProduct == other._orderProduct && _seqNo == other._seqNo);
        }

        public static bool operator ==(OrderCompartment left, OrderCompartment right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(OrderCompartment left, OrderCompartment right)
        {
            if (ReferenceEquals(left, right))
                return false;
            if (ReferenceEquals(left, null))
                return true;
            return !left.Equals(right);
        }

        #endregion
    }

    public enum LoadingCompartmentStatus
    {
        Free,
        Ready,
        Filling,
        Stopped,
        Completed,
        Partial,
        Aborted,
        Unused
    }
}
