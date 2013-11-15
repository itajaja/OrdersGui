using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
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
            set { Set("Orders", ref _orders, value); }
        }

        public LoadOrderManagerVM(IDataService ds)
        {
            try
            {
                _dataService = ds;
                _dataService.GetSessionData(
                    (item, error) =>
                    {
                        if (error != null)
                            throw error;
                        SessionData = item;
                    });
                _dataService.GetSystemData(
                    (item, error) =>
                    {
                        if (error != null)
                            throw error;
                        SystemInfo = item;
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
                throw;
            }
        }

        private void Reload(object sender, EventArgs eventArgs)
        {
            var t = new Task(() =>
            {
                //                    _dataService.GetOpcStatus(
                //                        (item, error) =>
                //                        {
                //                            if (error != null)
                //                                throw error;
                //                            DispatcherHelper.CheckBeginInvokeOnUI(() => _sessionData.OpcStatus = item);
                //                            DispatcherHelper.CheckBeginInvokeOnUI(() => _sessionData.SlomStatus = SlomConnectionStatus.Connected);
                //                                            });
                _dataService.GetOrders(
                    (item, error) =>
                    {
                        try
                        {
                            if (error != null)
                                throw error;
                            DispatcherHelper.CheckBeginInvokeOnUI(() => Orders = Orders.Union(Orders).ToList());    
                        }
                        
                    });
            });
            t.Start();
        }

        private void HandleException(Exception exception)
        {
            CurrentException = exception;
            SessionData = new SessionData
            {
                EmConnectionString = String.Empty,
                NtfConnectionString = String.Empty,
                OpcStatus = OpcConnectionStatus.Unknown,
                SlomStatus = SlomConnectionStatus.Disconnected,
                User = User.User0
            };

        }
    }
}