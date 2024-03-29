﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Data;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Hylasoft.OrdersGui.Messages;
using Hylasoft.OrdersGui.Model;
using Hylasoft.OrdersGui.Model.Service;
using Hylasoft.OrdersGui.Resources;
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
            get { return _dateFilter ?? DateTime.Today; }
            set
            {
                Set("DateFilter", ref _dateFilter, value);
                if (OrdersView != null)
                    OrdersView.Refresh();
            }
        }

        private readonly RelayCommand<Order> _orderSummaryCommand = new RelayCommand<Order>(
                order => HtmlPage.Window.Navigate(new Uri(string.Format(Configuration.OrderSummaryUrl, order.OrderId)), "blank"),
                order => order != null);
        private readonly RelayCommand<Order> _sampleLabelCommand = new RelayCommand<Order>(
                order => HtmlPage.Window.Navigate(new Uri(string.Format(Configuration.SampleLabelUrl, order.OrderId)), "blank"),
                order => order != null);
        private readonly RelayCommand<Order> _weightTicketCommand = new RelayCommand<Order>(
                order => HtmlPage.Window.Navigate(new Uri(string.Format(Configuration.WeightTicketUrl, order.OrderId)), "blank"),
                order => order != null);
        private readonly RelayCommand<Order> _truckInspectionCommand = new RelayCommand<Order>(
                order => HtmlPage.Window.Navigate(new Uri(string.Format(Configuration.TruckInspectionUrl, order.OrderId)), "blank"),
                order => order != null);
        private readonly RelayCommand<DateTime> _dailyDemandCommand = new RelayCommand<DateTime>(
                date => HtmlPage.Window.Navigate(new Uri(string.Format(Configuration.DailyDemandUrl, date.ToShortDateString())), "blank"));
        private readonly RelayCommand<DateTime> _pendingOrdersCommand = new RelayCommand<DateTime>(
                date => HtmlPage.Window.Navigate(new Uri(string.Format(Configuration.PendingOrdersUrl, date.ToShortDateString())), "blank"));
        public RelayCommand<Order> OrderSummaryCommand { get { return _orderSummaryCommand; } }
        public RelayCommand<Order> SampleLabelCommand { get { return _sampleLabelCommand; } }
        public RelayCommand<Order> WeightTicketCommand { get { return _weightTicketCommand; } }
        public RelayCommand<Order> TruckInspectionCommand { get { return _truckInspectionCommand; } }
        public RelayCommand<DateTime> DailyDemandCommand { get { return _dailyDemandCommand; } }
        public RelayCommand<DateTime> PendingOrdersCommand { get { return _pendingOrdersCommand; } }


        public RelayCommand ClearStatusFilerCommand { get; private set; }
        public RelayCommand AddAllStatusFilterCommand { get; private set; }
        public RelayCommand SetTodayFilterCommand { get; private set; }
        public RelayCommand ClearDateFilterCommand { get; private set; }
        //contextmenu command
        public RelayCommand<Order> ViewEditDetailsCommand { get; private set; }
        public RelayCommand<Order> PrepareOrderCommand { get; private set; }
        public RelayCommand<Order> FullfillOrderCommand { get; private set; }
        public RelayCommand<Order> EnterSealsCommand { get; private set; }
        public RelayCommand<Order> ReleaseCommand { get; private set; }

        public LoadOrderManagerVM(IDataService ds)
        {
            _dataService = ds;
            _dataService.GetSessionData(
                (item, error) =>
                {
                    HandleException(error);
                    DispatcherHelper.CheckBeginInvokeOnUI(() => SessionData = item);
                });
            _dataService.GetSystemData(
                (item, error) =>
                {
                    HandleException(error);
                    DispatcherHelper.CheckBeginInvokeOnUI(() => SystemInfo = item);
                });
            _dataService.GetOrders(
                (item, error) =>
                {
                    HandleException(error);
                    DispatcherHelper.CheckBeginInvokeOnUI(() => Orders = new ObservableCollection<Order>(item));
                });
            OrderStatusFilter = new TrulyObservableCollection<OrderStatusCheck>();
            InitializeCommands();
            foreach (var status in EnumList.GetEnumValues<OrderStatus>())
                OrderStatusFilter.Add(new OrderStatusCheck { Status = status, IsChecked = true });
            var updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromSeconds(int.Parse(Configuration.RefreshTime));
            updateTimer.Tick += Reload;
            updateTimer.Start();
            Reload(null, null);
        }

        private void Reload(object sender, EventArgs eventArgs)
        {
            _dataService.GetServerStatus(
                (itemStatus, slomStatus, error) => DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    try
                    {
                        if (error != null) throw error;

                        _sessionData.OpcStatus = itemStatus;
                        _sessionData.SlomStatus = slomStatus;
                    }
                    catch (Exception e)
                    {
                        HandleException(e);
                    }
                    finally
                    {
                        RefreshCanExecuteCommands();
                    }
                }));
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
            //            throw new Exception(exception.Message,exception);
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

            //ContextMenu commands
            ViewEditDetailsCommand = new RelayCommand<Order>(order =>
                Messenger.Default.Send(new GoToLodMessage
                {
                    Mode = order.OrderStatus == OrderStatus.Ready ? DetailMode.Edit : DetailMode.View,
                    Order = order
                }), order => order != null);
            PrepareOrderCommand = new RelayCommand<Order>(order =>
                Messenger.Default.Send(new GoToLodMessage
                {
                    Mode = DetailMode.Prepare,
                    Order = order
                }), order => order != null &&
                    (order.OrderStatus == OrderStatus.Ready || order.OrderStatus == OrderStatus.TruckArrived || order.OrderStatus == OrderStatus.ReadyForRelease ||
                    order.OrderType == OrderType.Load && (order.OrderStatus == OrderStatus.InspectionFailed || order.OrderStatus == OrderStatus.ReleaseError || order.OrderStatus == OrderStatus.Suspended)));
            FullfillOrderCommand = new RelayCommand<Order>(order =>
                Messenger.Default.Send(new GoToLodMessage
                {
                    Mode = DetailMode.Fullfill,
                    Order = order
                }), order => order != null && (order.OrderType == OrderType.Load && order.OrderStatus == OrderStatus.Released || order.OrderType == OrderType.Unload && order.OrderStatus == OrderStatus.Approved));
            //todo implement
            EnterSealsCommand = new RelayCommand<Order>(order => { MessageBox.Show("implement"); }, order => order != null && order.OrderType == OrderType.Load);
            ReleaseCommand = new RelayCommand<Order>(order => { MessageBox.Show("implement"); }, order => order != null && order.OrderType == OrderType.Load);

            _commandList = new List<RelayCommand>{
                ClearStatusFilerCommand, AddAllStatusFilterCommand,
                SetTodayFilterCommand, ClearDateFilterCommand
            };
        }

        private List<RelayCommand> _commandList;
        private void RefreshCanExecuteCommands()
        {
            foreach (var command in _commandList.Where(c => c != null))
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