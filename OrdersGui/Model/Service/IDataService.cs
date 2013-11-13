using System;
using System.Collections.Generic;

namespace Hylasoft.OrdersGui.Model.Service
{
    public interface IDataService
    {
        void GetSessionData(Action<SessionData, Exception> callback);
        void GetSystemData(Action<SystemInfo, Exception> callback);
        void GetRacks(Action<IList<Rack>, Exception> callback);
        void GetArms(Action<IList<Arm>, Exception> callback);
        void GetOrders(Action<IList<Order>, Exception> callback);
    }
}
