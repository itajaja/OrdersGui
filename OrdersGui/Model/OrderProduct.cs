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

        #region Equality code

        public override int GetHashCode()
        {
            return Material.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OrderProduct)obj);
        }

        public bool Equals(OrderProduct other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _material == other._material;
        }

        public static bool operator ==(OrderProduct left, OrderProduct right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(OrderProduct left, OrderProduct right)
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