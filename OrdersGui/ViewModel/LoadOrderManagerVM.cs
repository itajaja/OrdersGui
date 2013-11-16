using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using Hylasoft.OrdersGui.Model;
using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.ViewModel
{
    public class LoadOrderManagerVM : ViewModelBase
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

        private IList<Order> _orders;
        public IList<Order> Orders
        {
            get { return _orders; }
            set
            {
                Set("Orders", ref _orders, value);
                OrdersView = new PagedCollectionView(_orders);
                OrdersView.Filter = o =>
                {
                    var order = (Order)o;
                    if (!order.OrderNo.Contains(OrderNoFilter??""))
                        return false;
                    if (!_orderStatusFilter.Contains(order.OrderStatus))
                        return false;
                    if (DateFilter.HasValue && DateFilter.Value.Date.Equals(order.ScheduleDate.Date))
                        return false;
                    return true;
                };
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
                OrdersView.Refresh();
            }
        }

        private IList<OrderStatus> _orderStatusFilter;
        public IList<OrderStatus> OrderStatusFilter
        {
            get { return _orderStatusFilter; }
            set { Set("OrderStatusFilter", ref _orderStatusFilter, value); }
        }

        private DateTime? _dateFilter;
        public DateTime? DateFilter
        {
            get { return _dateFilter; }
            set { Set("DateFilter", ref _dateFilter, value); }
        }

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
                            Orders = item; //todo maybe add only the new ones?
                        }
                        catch (Exception e)
                        {
                            HandleException(e);
                        }
                    });
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
            throw exception;
        }
    }
}