namespace Hylasoft.OrdersGui.Model
{
    public class Arm : NotifyPropertyChanged
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
    }
}
