using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Hylasoft.OrdersGui.Model;
using Hylasoft.OrdersGui.Model.Service;
using Hylasoft.OrdersGui.Utils;

namespace Hylasoft.OrdersGui.ViewModel
{
    public sealed class CreateOrderVM : ViewModelBase
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

        private IList<DeliveryMethod> _methods = EnumList.GetEnumValues<DeliveryMethod>();
        public IList<DeliveryMethod> Methods
        {
            get { return _methods; }
            set { Set("Methods", ref _methods, value); }
        }

        private IList<OrderType> _types = EnumList.GetEnumValues<OrderType>();
        public IList<OrderType> Types
        {
            get { return _types; }
            set { Set("Types", ref _types, value); }
        }

        public CreateOrderVM(IDataService ds)
        {
            _dataService = ds;
            Cleanup();
        }

        public override void Cleanup()
        {
            base.Cleanup();
            Order = new Order();
            //initialize with 5 empty products
            OrderProducts = new List<OrderProduct>{
                new OrderProduct{Material = new Material(), SourceTank = new Tank()},
                new OrderProduct{Material = new Material(), SourceTank = new Tank()},
                new OrderProduct{Material = new Material(), SourceTank = new Tank()},
                new OrderProduct{Material = new Material(), SourceTank = new Tank()},
                new OrderProduct{SourceTank = new Tank()}
            };
        }
    }
}