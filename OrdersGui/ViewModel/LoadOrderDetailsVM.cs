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
        private readonly IDataService _dataservice;
        private SessionData _sessionData;


        private DetailMode _mode;
        public DetailMode Mode
        {
            get { return _mode; }
            set { Set("Mode", ref _mode, value); }
        }

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
        public RelayCommand FulfillOrderCommand { get; private set; }
        public RelayCommand ManualApproveCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        public LoadOrderDetailsVM(IDataService ds)
        {
            _dataservice = ds;
            _dataservice.GetSessionData((data, exception) => _sessionData = data);
            Messenger.Default.Register<GoToLodMessage>(this, o =>
            {
                RefreshCommands();
                _mode = o.Mode;
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
                        RefreshCommands();
                    }));
            });
            GoBackCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new GoToLomMessage());
                Reset();
            });
            //todo implement
            AssignCompartmentCommand = new RelayCommand(() => MessageBox.Show("assign comp"),
                () => CanExecute(OrderStatus.Ready));
            AssignTruckCommand = new RelayCommand(() => MessageBox.Show("assign truck"),
                () => CanExecute(OrderStatus.Ready));
            FulfillOrderCommand = new RelayCommand(() => MessageBox.Show("fullfill"),
                () => CanExecute(OrderStatus.Approved)
                );
            SaveCommand = new RelayCommand(() => MessageBox.Show("save"),
                () => CanExecute(DetailMode.Details)
                );
            ManualApproveCommand = new RelayCommand(() => MessageBox.Show("assign comp"),
                () => CanExecute(OrderStatus.PendingApproval));
            Reset();
        }

        private void RefreshCommands()
        {
            GoBackCommand.RaiseCanExecuteChanged();
            AssignCompartmentCommand.RaiseCanExecuteChanged();
            AssignTruckCommand.RaiseCanExecuteChanged();
            FulfillOrderCommand.RaiseCanExecuteChanged();
            ManualApproveCommand.RaiseCanExecuteChanged();
            SaveCommand.RaiseCanExecuteChanged();
        }

        private bool CanExecute(DetailMode mode)
        {
            return _mode == mode;
        }

        private bool CanExecute(OrderStatus status)
        {
            return Order != null && Order.OrderStatus == status;
        }

        public void Reset()
        {
            Order = null;
            OrderProducts = null;
            OrderCompartments = null;
            Compartments = null;
        }
    }
}