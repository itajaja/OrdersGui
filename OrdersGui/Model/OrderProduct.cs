using System;
using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.Model
{
    public class OrderProduct : NotifyPropertyChanged
    {
        private Tank _sourceTank;
        public Tank SourceTank
        {
            get { return _sourceTank; }
            set
            {
                SetField(ref _sourceTank, value, "SourceTank");
                OnPropertyChanged("PoundsQuantity");
                OnPropertyChanged("TargetQty");
            }
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
            set
            {
                if (_uom == SystemInfo.Gallons && value == SystemInfo.Pounds)
                    PoundsQuantity = _targetQty;
                if (_uom == SystemInfo.Pounds && value == SystemInfo.Gallons)
                    TargetQty = PoundsQuantity;
                SetField(ref _uom, value, "Uom", "PoundsQuantity", "TargetQty");
            }
        }

        public double Density
        {
            get
            {
                return CalculateDensity();
            }
        }

        private double _targetQty;
        public double TargetQty
        {
            get { return _targetQty; }
            set
            {
                SetField(ref _targetQty, value, "TargetQty", "PoundsQuantity");
            }
        }

        public double PoundsQuantity
        {
            get { return TargetQty / Density; }
            set
            {
                SetField(ref _targetQty, Density*value, "PoundsQuantity", "TargetQty");
            }
        }

        private double CalculateDensity()
        {
            try
            {
                var d = (141.5/(SourceTank.ApiGravity + 131.5))*8.328;
                return d <= 0 ? Double.NaN : d;
            }
            catch
            {
                return Double.NaN;
            }
        }
    }
}