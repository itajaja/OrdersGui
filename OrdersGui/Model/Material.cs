using System.Collections.Generic;
using System.Linq;
using Hylasoft.OrdersGui.Model.Service;
using Hylasoft.OrdersGui.ViewModel;

namespace Hylasoft.OrdersGui.Model
{
    public class Material : NotifyPropertyChanged
    {
        public const int RebrandedCategory = 2;

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

        private int _materialFamily;
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

        /// <summary>
        /// Find all the tanks that can contain the material
        /// </summary>
        /// <param name="tanks">The entire list of tanks</param>
        /// <returns>The tanks that can contain the material</returns>
        public virtual IList<Tank> FindTanks(IList<Tank> tanks)
        {
            return tanks.Where(t => t.Material == this || t.TankName.ToLower() == AssignCompartmentsVM.UnknownTank).ToList();
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
