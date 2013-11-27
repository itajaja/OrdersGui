using System.ServiceModel.Channels;
using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.Model
{
    public class RebrandedProduct : NotifyPropertyChanged
    {
        private string _materialCode;
        public string MaterialCode
        {
            get { return _materialCode; }
            set { SetField(ref _materialCode, value, "MaterialCode"); }
        }

        private int _materialId;
        public int MaterialId
        {
            get { return _materialId; }
            set { SetField(ref _materialId, value, "MaterialId"); }
        }

        private Material _parent;
        public Material Parent
        {
            get { return _parent; }
            set { SetField(ref _parent, value, "Parent"); }
        }

        #region Equality code

        public override int GetHashCode()
        {
            return MaterialId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RebrandedProduct)obj);
        }

        public bool Equals(RebrandedProduct other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _materialId == other._materialId;
        }

        public static bool operator ==(RebrandedProduct left, RebrandedProduct right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(RebrandedProduct left, RebrandedProduct right)
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
