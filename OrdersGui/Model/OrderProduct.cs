using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.Model
{
    public class OrderProduct : NotifyPropertyChanged
    {
        private Tank _sourceTank;
        public Tank SourceTank
        {
            get { return _sourceTank; }
            set { SetField(ref _sourceTank, value, "SourceTank"); }
        }

        private double _targetQty;
        public double TargetQty
        {
            get { return _targetQty; }
            set { SetField(ref _targetQty, value, "TargetQty"); }
        }

        private Material _material;
        public Material Material
        {
            get { return _material; }
            set { SetField(ref _material, value, "Material"); }
        }

        private string _uom;
        public string Uom
        {
            get { return _uom; }
            set { SetField(ref _uom, value, "Uom"); }
        }
    }
}