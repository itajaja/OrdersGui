using System.Collections.Generic;
using System.Linq;

namespace Hylasoft.OrdersGui.Model
{
    public class RebrandedProduct : Material
    {
        public RebrandedProduct() { }

        public RebrandedProduct(Material m)
        {
            CategoryName = m.CategoryName;
            DataEntryValue = m.DataEntryValue;
            MaterialCategory = m.MaterialCategory;
            MaterialCode = m.MaterialCode;
            MaterialFamily = m.MaterialFamily;
            MaterialId = m.MaterialId;
            MaterialName = m.MaterialName;
        }

        private Material _parent;
        public Material Parent
        {
            get { return _parent; }
            set { SetField(ref _parent, value, "Parent"); }
        }

        /// <summary>
        /// Find all the tanks that can contain the material (looking also through parents)
        /// </summary>
        /// <param name="tanks">The entire list of tanks</param>
        /// <returns>The tanks that can contain the material</returns>
        public override IList<Tank> FindTanks(IList<Tank> tanks)
        {
            IEnumerable<Tank> p = base.FindTanks(tanks);
            if (Parent != null)
                p = p.Union(Parent.FindTanks(tanks));
            return p.ToList();
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
            return MaterialId == other.MaterialId;
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
