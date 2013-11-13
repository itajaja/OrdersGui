using System;
using System.Collections.Generic;
using System.Linq;
using Hylasoft.OrdersGui.Model;
using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.Design
{
    public partial class DesignDataService : IDataService
    {
        public void GetSessionData(Action<SessionData, Exception> callback)
        {
            // Use this to create design time data
            var data = new SessionData{ConnectionString = "localhost", OpcStatus = ConnectionStatus.Connected,
                SlomStatus = ConnectionStatus.Disconnected, User = User.User2};
            callback(data, null);
        }

        public void GetSystemData(Action<Model.SystemInfo, Exception> callback)
        {

            callback(_data, null);
        }

        public void GetRacks(Action<IList<Rack>, Exception> callback)
        {
            callback(_racks, null);
        }

        public void GetArms(Action<IList<Arm>, Exception> callback)
        {
            callback(_arms, null);
        }

        public void GetOrders(Action<IList<Order>, Exception> callback)
        {
            callback(_orders, null);
        }
    }
}
