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
    public sealed class LoadOrderDetailsVM : ViewModelBase
    {
        private IDataService _dataservice;
        private bool _readOnly;
        private SessionData _sessionData;

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

        private TrulyObservableCollection<OrderCompartment> _orderCompartments;
        public TrulyObservableCollection<OrderCompartment> OrderCompartments
        {
            get { return _orderCompartments; }
            set { Set("OrderCompartments", ref _orderCompartments, value); }
        }

        private TrulyObservableCollection<Compartment> _compartments;
        public TrulyObservableCollection<Compartment> Compartments
        {
            get { return _compartments; }
            set { Set("Compartments", ref _compartments, value); }
        }

        private Container _container;
        public Container Container
        {
            get { return _container; }
            set { Set("Container", ref _container, value); }
        }

        public RelayCommand GoBackCommand { get; private set; }
        public RelayCommand AssignCompartmentCommand { get; private set; }
        public RelayCommand AssignTruckCommand { get; private set; }

        public LoadOrderDetailsVM(IDataService ds)
        {
            _dataservice = ds;
            _dataservice.GetSessionData((data, exception) => _sessionData = data);

            Messenger.Default.Register<GoToLodMessage>(this, o =>
            {
                _readOnly = o.IsReadOnly;
                Order = o.Order;
                _dataservice.GetOrderDetails(Order.OrderId,
                    (orderProds, orderComps, comps, container, error) => DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        if (orderProds != null)
                            OrderProducts = new TrulyObservableCollection<OrderProduct>(orderProds);
                        if (orderComps != null)
                        {
                            OrderCompartments = new TrulyObservableCollection<OrderCompartment>(orderComps);
                            Compartments = new TrulyObservableCollection<Compartment>(comps);
                            Container = container;
                        }
                    })
                    );
            });
            GoBackCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new GoToLomMessage());
                Reset();
            });
            AssignCompartmentCommand = new RelayCommand(() => { },
                CanExecute);
            AssignTruckCommand = new RelayCommand(() => { },
                CanExecute);
            Reset();
        }

        private bool CanExecute()
        {
            return _sessionData.User != User.User0 && !_readOnly;
        }

        public void Reset()
        {
            Order = new Order();
            //initialize with 5 empty products and compartments
            OrderProducts = new TrulyObservableCollection<OrderProduct>{
                new OrderProduct(),
                new OrderProduct(),
                new OrderProduct(),
                new OrderProduct(),
                new OrderProduct()
            };
            OrderCompartments = new TrulyObservableCollection<OrderCompartment>{
                new OrderCompartment(),
                new OrderCompartment(),
                new OrderCompartment(),
                new OrderCompartment(),
                new OrderCompartment()
            };
        }
    }
}