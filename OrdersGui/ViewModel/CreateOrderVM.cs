using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Hylasoft.OrdersGui.Messages;
using Hylasoft.OrdersGui.Model;
using Hylasoft.OrdersGui.Model.Service;
using Hylasoft.OrdersGui.Utils;

namespace Hylasoft.OrdersGui.ViewModel
{
    public sealed class CreateOrderVM : ViewModelBase
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly IDataService _dataService;

        private Order _order;
        public Order Order
        {
            get { return _order; }
            set { Set("Order", ref _order, value); }
        }

        private TrulyObservableCollection<OrderProduct> _orderProducts;
        public TrulyObservableCollection<OrderProduct> OrderProducts
        {
            get { return _orderProducts; }
            set { Set("OrderProducts", ref _orderProducts, value); }
        }

        private ObservableCollection<Tank> _tanks;
        public ObservableCollection<Tank> Tanks
        {
            get { return _tanks; }
            set { Set("Tanks", ref _tanks, value); }
        }

        private ObservableCollection<Material> _materials;
        public ObservableCollection<Material> Materials
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

        private ObservableCollection<string> _uoms = new ObservableCollection<string>{"GAL","LB"};
        public ObservableCollection<string> Uoms
        {
            get { return _uoms; }
            set { Set("Uoms", ref _uoms, value); }
        }

        public RelayCommand CreateOrderCommand { get; private set; }
        public RelayCommand GoBackCommand { get; private set; }
        public RelayCommand<OrderProduct> ClearRowCommand { get; private set; }

        public CreateOrderVM(IDataService ds)
        {
            _dataService = ds;
            //todo need a smart way to handle errors
            _dataService.GetMaterials((item, error) => DispatcherHelper.CheckBeginInvokeOnUI(() => Materials = new ObservableCollection<Material>(item)));
            _dataService.GetSapTanks((item, error) => DispatcherHelper.CheckBeginInvokeOnUI(() => Tanks = new ObservableCollection<Tank>(item)));
            CreateOrderCommand = new RelayCommand(() =>
            {
                if (Validate())
                    MessageBox.Show("YEAH"); //todo implement
            });
            GoBackCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new GoToLomMessage());
                Reset();
            });
            ClearRowCommand = new RelayCommand<OrderProduct>(product =>
            {
                product.Material = null;
                product.SourceTank = null;
                product.TargetQty = 0;
                product.Uom = null;
            });
            Reset();
        }

        public void Reset()
        {
            Order = new Order();
            //initialize with 5 empty products
            OrderProducts = new TrulyObservableCollection<OrderProduct>{
                new OrderProduct(),
                new OrderProduct(),
                new OrderProduct(),
                new OrderProduct(),
                new OrderProduct()
            };
        }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(Order.OrderNo))
            {
                MessageBox.Show("The order number field cannot be empty.");
                return false;
            }
            //todo if (Orders.Any(o => o.OrderNo == order.OrderNo))
            var uniqueProductList = new List<string>();
            var validProducts = OrderProducts.Where(product => !IsEmptyProduct(product)).ToList();
            if (validProducts.Count == 0)
            {
                MessageBox.Show("You must insert at least a product");
                return false;
            }
            foreach (var p in validProducts)
            {
                bool containsItem = uniqueProductList.Any(item => item == p.Material.MaterialCode);
                if (containsItem)
                {
                    MessageBox.Show(p.Material.MaterialName + " should only be entered once.", "ERROR", MessageBoxButton.OK);
                    return false;
                }
                uniqueProductList.Add(p.Material.MaterialCode);
                if (p.SourceTank == null || string.IsNullOrEmpty(p.SourceTank.SapTankName))
                {
                    MessageBox.Show("Please insert a tank for " + p.Material.MaterialName, "ERROR", MessageBoxButton.OK);
                    return false;
                }
                var tank = Tanks.FirstOrDefault(t => p.SourceTank.SapTankName == t.SapTankName);
                if (tank == null)
                {
                    MessageBox.Show("Tank " + p.SourceTank + " doesn't exist", "ERROR", MessageBoxButton.OK);
                    return false;
                }
                if (p.Material.MaterialId != tank.Material.MaterialId)
                {
                    var result = MessageBox.Show("The tank entered for " + p.Material.MaterialName + " contains a material other than the one selected.  Would you like to save anyway?", "ERROR", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.Cancel)
                        return false;
                }
                if (p.TargetQty <= 0)
                {
                    MessageBox.Show("The quantity for " + p.Material.MaterialName + " must be greater than 0", "ERROR", MessageBoxButton.OK);
                    return false;
                }
                if (p.Uom != "LB" && p.Uom != "GAL")
                {
                    MessageBox.Show(p.Material.MaterialName + " unit of measure not set to GAL for gallons or LB for pounds", "ERROR", MessageBoxButton.OK);
                    return false;
                }
            }
            return true;
        }

        private bool IsEmptyProduct(OrderProduct op)
        {
            return op.Material == null
                   && op.SourceTank == null
                   && op.TargetQty <= 0
                   && string.IsNullOrEmpty(op.Uom);
        }
    }
}