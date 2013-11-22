using System.ServiceModel.Channels;
using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.Model
{
    public class Material : NotifyPropertyChanged
    {
        private string _categoryName;
        public string CategoryName
        {
            get { return _categoryName; }
            set { SetField(ref _categoryName, value, "CategoryName"); }
        }

        private double _dataEntryValue;
        public double DataEntryValue
        {
            get { return _dataEntryValue; }
            set { SetField(ref _dataEntryValue, value, "DataEntryValue"); }
        }

        private int _materialCategory;
        public int MaterialCategory
        {
            get { return _materialCategory; }
            set { SetField(ref _materialCategory, value, "MaterialCategory"); }
        }

        private string _materialCode;
        public string MaterialCode
        {
            get { return _materialCode; }
            set { SetField(ref _materialCode, value, "MaterialCode"); }
        }

        private int _materialFamily; //todo maybe ref
        public int MaterialFamily
        {
            get { return _materialFamily; }
            set { SetField(ref _materialFamily, value, "MaterialFamily"); }
        }

        private int _materialId;
        public int MaterialId
        {
            get { return _materialId; }
            set { SetField(ref _materialId, value, "MaterialId"); }
        }

        private string _materialName;
        public string MaterialName
        {
            get { return _materialName; }
            set { SetField(ref _materialName, value, "MaterialName"); }
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
            return Equals((Material)obj);
        }

        public bool Equals(Material other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _materialId == other._materialId;
        }

        public static bool operator ==(Material left, Material right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(Material left, Material right)
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
