using System;
using System.Collections.Generic;

namespace Hylasoft.OrdersGui.Model.Service
{
    public class DataService : IDataService
    {
        private readonly SessionData _sessionData = new SessionData();
        private readonly SystemInfo _systemInfoData = new SystemInfo();

        public DataService()
        {
            //todo initialize sessiondata, start timers, etc
        }

        public void GetSessionData(Action<SessionData, Exception> callback)
        {
            callback(_sessionData, null);
        }

        public void GetSystemData(Action<SystemInfo, Exception> callback)
        {
            callback(_systemInfoData, null);
        }

        public void GetRacks(Action<IList<Rack>, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void GetArms(Action<IList<Arm>, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void GetOrders(Action<IList<Order>, Exception> callback)
        {
            throw new NotImplementedException();
        }
    }
}