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
        void GetOpcStatus(Action<OpcConnectionStatus, Exception> callback);
        void GetMaterials(Action<IList<Material>, Exception> callback);
        void GetTanks(Action<IList<Tank>, Exception> callback);
        void GetSapTanks(Action<IList<Tank>, Exception> callback);
        void GetCompartments(Action<IList<Compartment>, Exception> callback);
        void GetContainers(Action<IList<Container>, Exception> callback);
        void GetOrderProducts(long orderId, Action<IList<OrderCompartment>, Exception> callback);
        void GetOrderCompartments(long orderId, Action<IList<OrderProduct>, Exception> callback);

        void CreateOrder(Action<Exception> callback);
    }
}
