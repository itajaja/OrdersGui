using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Hylasoft.OrdersGui.Model;
using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.ViewModel
{
    public sealed class LoadOrderDetailsVM : ViewModelBase
    {
        private IDataService _dataservice;

        private Order _order;
        public Order Order
        {
            get { return _order; }
            set { Set("Order", ref _order, value); }
        }

        private IList<OrderProduct> _orderProducts;
        public IList<OrderProduct> OrderProducts
        {
            get { return _orderProducts; }
            set { Set("OrderProducts", ref _orderProducts, value); }
        }

        private IList<OrderCompartment> _orderCompartments;
        public IList<OrderCompartment> OrderCompartments
        {
            get { return _orderCompartments; }
            set { Set("OrderCompartments", ref _orderCompartments, value); }
        }

        /// <summary>
        /// Initializes a new instance of the LoadOrderDetailsVM class.
        /// </summary>
        public LoadOrderDetailsVM(IDataService ds)
        {
            _dataservice = ds;
        }
    }
}