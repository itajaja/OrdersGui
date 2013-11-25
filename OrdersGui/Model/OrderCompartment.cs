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

        private SequenceNumber _seqNo;
        public SequenceNumber SeqNo
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

        public OrderCompartment Clone()
        {
            return (OrderCompartment) MemberwiseClone();
        }
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

    public enum SequenceNumber
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        E = 5
    }
}
