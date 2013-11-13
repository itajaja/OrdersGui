using System;

namespace Hylasoft.OrdersGui.Model
{
    public class Order : NotifyPropertyChanged
    {
        private string _carrierInstruction;
        public string CarrierInstruction
        {
            get { return _carrierInstruction; }
            set { SetField(ref _carrierInstruction, value, "CarrierInstruction"); }
        }

        private string _carrierName;
        public string CarrierName
        {
            get { return _carrierName; }
            set { SetField(ref _carrierName, value, "CarrierName"); }
        }

        private string _customerName;
        public string CustomerName
        {
            get { return _customerName; }
            set { SetField(ref _customerName, value, "CustomerName"); }
        }

        private string _customerNo;
        public string CustomerNo
        {
            get { return _customerNo; }
            set { SetField(ref _customerNo, value, "CustomerNo"); }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set { SetField(ref _endDate, value, "EndDate"); }
        }

        private int _loadRackId;
        public int LoadRackId //todo model
        {
            get { return _loadRackId; }
            set { SetField(ref _loadRackId, value, "LoadRackId"); }
        }

        private DeliveryMethod _methodOfDelivery;
        public DeliveryMethod MethodOfDelivery
        {
            get { return _methodOfDelivery; }
            set { SetField(ref _methodOfDelivery, value, "MethodOfDelivery"); }
        }

        private string _methodOfDeliveryDescription;
        public string MethodOfDeliveryDescription
        {
            get { return _methodOfDeliveryDescription; }
            set { SetField(ref _methodOfDeliveryDescription, value, "MethodOfDeliveryDescription"); }
        }

        private string _note;
        public string Note
        {
            get { return _note; }
            set { SetField(ref _note, value, "Note"); }
        }

        private long _orderId;
        public long OrderId
        {
            get { return _orderId; }
            set { SetField(ref _orderId, value, "OrderId"); }
        }

        private string _orderKey;
        public string OrderKey
        {
            get { return _orderKey; }
            set { SetField(ref _orderKey, value, "OrderKey"); }
        }

        private string _orderNo;
        public string OrderNo
        {
            get { return _orderNo; }
            set { SetField(ref _orderNo, value, "OrderNo"); }
        }

        private OrderStatus _orderStatus;
        public OrderStatus OrderStatus
        {
            get { return _orderStatus; }
            set { SetField(ref _orderStatus, value, "OrderStatus"); }
        }

        private OrderType _orderType;
        public OrderType OrderType
        {
            get { return _orderType; }
            set { SetField(ref _orderType, value, "OrderType"); }
        }

        private string _poNumber;
        public string PoNumber
        {
            get { return _poNumber; }
            set { SetField(ref _poNumber, value, "PoNumber"); }
        }

        private DateTime _scheduleDate;
        public DateTime ScheduleDate
        {
            get { return _scheduleDate; }
            set { SetField(ref _scheduleDate, value, "ScheduleDate"); }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetField(ref _startDate, value, "StartDate"); }
        }

        private string _truckNo;
        public string TruckNo
        {
            get { return _truckNo; }
            set { SetField(ref _truckNo, value, "TruckNo"); }
        }
    }


    public enum OrderType
    {
        Load,
        Unload
    }

    public enum DeliveryMethod
    {
        Road,
        Rail,
        Undefined
    }

    public enum OrderStatus
    {
        Idle = 0,
        Ready = 1,
        TruckArrived = 2,
        ReadyForRelease = 3,
        ReleaseInProgress = 4,
        Released = 5,
        ReleaseError = 6,
        Running = 7,
        Stopped = 8,
        Completed = 9,
        Aborted = 10,
        Cancelled = 11,
        Suspended = 12,
        Approved = 13,
        Rejected = 14,
        WaitingForSeals = 15,
        EndFilling = 16,
        ReportReady = 17,
        ReportRetry = 18,
        UpdateDone = 19,
        Setup = 20,
        InspectionFailed = 21,
        PendingApproval = 22,
        SapUpdateDone = 23
    }
}
