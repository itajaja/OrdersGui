using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Hylasoft.OrdersGui.Model;
using Hylasoft.OrdersGui.Model.Service;
using Hylasoft.OrdersGui.Utils;

namespace Hylasoft.OrdersGui.ViewModel
{
    public sealed class LoadOrderManagerVM : ViewModelBase
    {
        private readonly IDataService _dataService;

        private Exception _currentException;
        public Exception CurrentException
        {
            get { return _currentException; }
            set { Set("CurrentException", ref _currentException, value); }
        }

        private SessionData _sessionData;
        public SessionData SessionData
        {
            get { return _sessionData; }
            set { Set("SessionData", ref _sessionData, value); }
        }

        private SystemInfo _systemInfo;
        public SystemInfo SystemInfo
        {
            get { return _systemInfo; }
            set { Set("SystemInfo", ref _systemInfo, value); }
        }

        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            set
            {
                Set("Orders", ref _orders, value);
                OrdersView = new PagedCollectionView(_orders);
                OrdersView.Filter = OrderFilter;
            }
        }

        private PagedCollectionView _ordersView;
        public PagedCollectionView OrdersView
        {
            get { return _ordersView; }
            set { Set("OrdersView", ref _ordersView, value); }
        }

        private string _orderNoFilter;
        public string OrderNoFilter
        {
            get { return _orderNoFilter; }
            set
            {
                Set("OrderNoFilter", ref _orderNoFilter, value);
                if (OrdersView != null)
                    OrdersView.Refresh();
            }
        }

        private TrulyObservableCollection<OrderStatusCheck> _orderStatusFilter;
        public TrulyObservableCollection<OrderStatusCheck> OrderStatusFilter
        {
            get { return _orderStatusFilter; }
            set
            {
                Set("OrderStatusFilter", ref _orderStatusFilter, value);
                OrderStatusFilter.CollectionChanged += (sender, args) =>
                {
                    if (OrdersView != null)
                        OrdersView.Refresh();
                };
            }
        }

        private DateTime? _dateFilter;
        public DateTime DateFilter
        {
            get { return _dateFilter??DateTime.Today; }
            set
            {
                Set("DateFilter", ref _dateFilter, value);
                if (OrdersView != null)
                    OrdersView.Refresh();
            }
        }

        public RelayCommand ClearStatusFilerCommand { get; private set; }
        public RelayCommand AddAllStatusFilterCommand { get; private set; }
        public RelayCommand SetTodayFilterCommand { get; private set; }
        public RelayCommand ClearDateFilterCommand { get; private set; }

        public LoadOrderManagerVM(IDataService ds)
        {
            try
            {
                _dataService = ds;
                _dataService.GetSessionData(
                    (item, error) =>
                    {
                        try
                        {
                            if (error != null) throw error;
                            SessionData = item;
                        }
                        catch (Exception e)
                        {
                            HandleException(e);
                            MessageBox.Show("Unhandled Error when opening LoadOrderManager. Please Restart the application.\n\n" + e);
                        }
                    });
                _dataService.GetSystemData(
                    (item, error) =>
                    {
                        try
                        {
                            if (error != null) throw error;
                            SystemInfo = item;
                        }
                        catch (Exception e)
                        {
                            HandleException(e);
                            MessageBox.Show("Unhandled Error when opening LoadOrderManager. Please Restart the application.\n\n" + e);
                        }
                    });
                _dataService.GetOrders(
                    (item, error) =>
                    {
                        try
                        {
                            if (error != null)
                                throw error;
                            Orders = new ObservableCollection<Order>(item); //todo maybe add only the new ones?
                        }
                        catch (Exception e)
                        {
                            HandleException(e);
                        }
                    });
                OrderStatusFilter = new TrulyObservableCollection<OrderStatusCheck>();
                InitializeCommands();
                foreach (var status in EnumList.GetEnumValues<OrderStatus>())
                    OrderStatusFilter.Add(new OrderStatusCheck{Status = status, IsChecked = true});
                var updateTimer = new DispatcherTimer();
                updateTimer.Interval = TimeSpan.FromSeconds(5); //todo resourcify
                updateTimer.Tick += Reload;
                updateTimer.Start();
                Reload(null, null);
            }
            catch (Exception e)
            {
                HandleException(e);
                MessageBox.Show("Unhandled Error when opening LoadOrderManager. Please Restart the application.\n\n" + e);
            }
        }

        private void Reload(object sender, EventArgs eventArgs)
        {
            _dataService.GetOpcStatus(
                (item, error) =>
                {
                    try
                    {
                        if (error != null) throw error;
                        _sessionData.OpcStatus = item;
                        _sessionData.SlomStatus = SlomConnectionStatus.Connected;
                        RefreshCanExecuteCommands();
                    }
                    catch (Exception e)
                    {
                        HandleException(e);
                    }
                });
        }

        private void HandleException(Exception exception)
        {
            if (exception == null)
                return;
            CurrentException = exception;
            SessionData = new SessionData
            {
                EmConnectionString = String.Empty,
                NtfConnectionString = String.Empty,
                OpcStatus = OpcConnectionStatus.Unknown,
                SlomStatus = SlomConnectionStatus.Disconnected,
                User = User.User0
            };
            throw new Exception(exception.Message,exception);
        }

        private bool OrderFilter(object o)
        {
            var order = (Order)o;
            if (!order.OrderNo.Contains(OrderNoFilter ?? ""))
                return false;
            if (_orderStatusFilter != null && _orderStatusFilter.All(oc => !oc.IsChecked || oc.Status != order.OrderStatus))
                return false;
            if (_dateFilter.HasValue && !_dateFilter.Value.Date.Equals(order.ScheduleDate.Date))
                return false;
            return true;
        }

        private void InitializeCommands()
        {
            ClearStatusFilerCommand = new RelayCommand(() =>
            {
                foreach (var status in OrderStatusFilter)
                    status.IsChecked = false;
            }, CanExecute);
            AddAllStatusFilterCommand = new RelayCommand(() =>
            {
                foreach (var status in OrderStatusFilter)
                    status.IsChecked = true;
            }, CanExecute);
            ClearDateFilterCommand = new RelayCommand(() =>
            {
                _dateFilter = null;
                if (OrdersView != null)
                    OrdersView.Refresh();
            }, CanExecute);
            SetTodayFilterCommand = new RelayCommand(() =>
            {
                DateFilter = DateTime.Today;
            }, CanExecute);
            _commandList = new List<RelayCommand>{
                ClearStatusFilerCommand, AddAllStatusFilterCommand,
                SetTodayFilterCommand, ClearDateFilterCommand
            };
        }

        private List<RelayCommand> _commandList; 
        private void RefreshCanExecuteCommands()
        {
            foreach (var command in _commandList.Where(c => c!=null))
                command.RaiseCanExecuteChanged();
        }

        private bool CanExecute()
        {
            return SessionData.SlomStatus == SlomConnectionStatus.Connected;
        }
    }

    public class OrderStatusCheck : NotifyPropertyChanged
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { SetField(ref _isChecked, value, "IsChecked"); }
        }

        private OrderStatus _status;
        public OrderStatus Status
        {
            get { return _status; }
            set { SetField(ref _status, value, "Status"); }
        }
    }
}