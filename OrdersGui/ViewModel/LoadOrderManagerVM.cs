using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Hylasoft.OrdersGui.Model;
using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.ViewModel
{
    public class LoadOrderManagerVM : ViewModelBase
    {
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
        
        public LoadOrderManagerVM(IDataService dataService)
        {
            dataService.GetSessionData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }
                    SessionData = item;
                });
            dataService.GetSystemData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }
                    SystemInfo = item;
                });
            dataService.GetOrders(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }
                    Orders = item;
                });
        }
    }
}