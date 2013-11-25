using System.Linq;
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
    public sealed class AssignCompartmentsVM : ViewModelBase
    {
        private readonly IDataService _dataservice;
        private DetailMode _cachedMode;

        private Order _order;
        public Order Order
        {
            get { return _order; }
            set { Set("Order", ref _order, value); }
        }

        private TrulyObservableCollection<OrderCompartment> _orderCompartments;
        public TrulyObservableCollection<OrderCompartment> OrderCompartments
        {
            get { return _orderCompartments; }
            set { Set("OrderCompartments", ref _orderCompartments, value); }
        }

        private TrulyObservableCollection<OrderProduct> _orderProducts;
        public TrulyObservableCollection<OrderProduct> OrderProducts
        {
            get { return _orderProducts; }
            set { Set("OrderProducts", ref _orderProducts, value); }
        }

        private TrulyObservableCollection<Compartment> _compartments;
        public TrulyObservableCollection<Compartment> Compartments
        {
            get { return _compartments; }
            set
            {
                Set("Compartments", ref _compartments, value);
                RefreshCommands();
            }
        }

        private string _errorString;
        public string ErrorString
        {
            get { return _errorString; }
            set { Set("ErrorString", ref _errorString, value); }
        }

        private Container _container;
        public Container Container
        {
            get { return _container; }
            set
            {
                Set("Container", ref _container, value);
                Compartments = null;
                if (_container != null)
                    _dataservice.GetCompartments(_container.ContainerId, (list, exception) => DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        Compartments = new TrulyObservableCollection<Compartment>(list);
                    }));
                RefreshCommands();
            }
        }
        

        public RelayCommand GoBackCommand { get; private set; }

        public AssignCompartmentsVM(IDataService ds, LoadOrderDetailsVM lodVM)
        {
            _dataservice = ds;
            Messenger.Default.Register<GoToAcMessage>(this, message =>
            {
                if (message.GoBack)
                    return;
                Order = lodVM.Order.Clone();
                Compartments = lodVM.Compartments;
                if (lodVM.OrderProducts != null)
                    OrderProducts = new TrulyObservableCollection<OrderProduct>(lodVM.OrderProducts.Select(o => o.Clone()));
                if (lodVM.Container != null)
                    Container = lodVM.Container.Clone();
                if (lodVM.OrderCompartments != null)
                    OrderCompartments = new TrulyObservableCollection<OrderCompartment>(lodVM.OrderCompartments.Select(o => o.Clone()));
                _cachedMode = lodVM.Mode;
                lodVM.Mode = DetailMode.View;
            });
            GoBackCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new GoToAcMessage(true));
                lodVM.Mode = _cachedMode;
                Reset();
            });
            Reset();
        }

        private void RefreshCommands()
        {
        }

        public void Reset()
        {
            Order = null;
            OrderCompartments = null;
            OrderProducts = null;
            Compartments = null;
            Container = null;
        }
    }

}