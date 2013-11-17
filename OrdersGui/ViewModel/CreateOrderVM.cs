using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Hylasoft.OrdersGui.Model;
using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.ViewModel
{
    public class CreateOrderVM : ViewModelBase
    {
        private readonly IDataService _dataService;

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

        private IList<Tank> _tanks;
        public IList<Tank> Tanks
        {
            get { return _tanks; }
            set { Set("Tanks", ref _tanks, value); }
        }

        private IList<Material> _materials;
        public IList<Material> Materials
        {
            get { return _materials; }
            set { Set("Materials", ref _materials, value); }
        }

        public CreateOrderVM(IDataService ds)
        {
            _dataService = ds;
        }
    }
}