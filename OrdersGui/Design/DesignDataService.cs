using System;
using System.Collections.Generic;
using Hylasoft.OrdersGui.Model;
using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.Design
{
    public partial class DesignDataService : IDataService
    {
        public void GetSessionData(Action<SessionData, Exception> callback)
        {
            // Use this to create design time data
            var data = new SessionData{EmConnectionString = "localhost", NtfConnectionString = "localhost", OpcStatus = OpcConnectionStatus.Running,
                SlomStatus = SlomConnectionStatus.Disconnected, User = User.User2};
            callback(data, null);
        }

        public void GetSystemData(Action<SystemInfo, Exception> callback)
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

        public void GetOpcStatus(Action<OpcConnectionStatus, Exception> callback)
        {
            callback(OpcConnectionStatus.Suspended, null);
        }

        public void GetMaterials(Action<IList<Material>, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void GetTanks(Action<IList<Tank>, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void GetCompartments(long containerId, Action<IList<Compartment>, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void GetContainers(Action<IList<Container>, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void GetOrderDetails(long orderId, Action<IList<OrderProduct>, IList<OrderCompartment>, IList<Compartment>, Container, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void CreateOrder(Action<Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void GetSapTanks(Action<IList<Tank>, Exception> callback)
        {
            throw new NotImplementedException();
        }
    }
}
