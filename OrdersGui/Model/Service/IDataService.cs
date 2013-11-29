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
        void GetServerStatus(Action<OpcConnectionStatus, SlomConnectionStatus, Exception> callback);
        void GetMaterials(Action<IList<Material>, Exception> callback);
        void GetTanks(Action<IList<Tank>, Exception> callback);
        void GetSapTanks(Action<IList<Tank>, Exception> callback);
        void GetCompartments(long containerId, Action<IList<Compartment>, Exception> callback);
        void GetContainers(Action<IList<Container>, Exception> callback);
        void GetOrderDetails(long orderId, Action<IList<OrderProduct>, IList<OrderCompartment>, IList<Compartment>, Container, Exception> callback);

        void CreateOrder(Order order, IList<OrderProduct> orderProducts, Action<Exception> callback);
    }
}
