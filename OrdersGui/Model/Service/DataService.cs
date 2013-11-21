using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using Hylasoft.OrdersGui.EventMonitor;
using Hylasoft.OrdersGui.Messages;
using Hylasoft.OrdersGui.NonTransactionalFunctions;
using Hylasoft.OrdersGui.Resources;

// ReSharper disable ImplicitlyCapturedClosure
namespace Hylasoft.OrdersGui.Model.Service
{
    public partial class DataService : IDataService
    {
        private readonly EventMonitorClient _emClient;
        private readonly NonTransactionalFunctionsClient _ntfClient;

        // These properties are stored into the dataService because they don't need to be fetched more than once
        private readonly SessionData _sessionData = new SessionData();
        private SystemInfo _systemInfo;
        private IList<Rack> _racks;
        private IList<Arm> _arms;
        private IList<Material> _materials;
        private IList<Tank> _sapTanks;
        private IList<Tank> _tanks;
        private List<Container> _containers;

        public DataService()
        {
            try
            {
                _sessionData = new SessionData();
                _sessionData.EmConnectionString = Configuration.EmConnectionString;
                _sessionData.NtfConnectionString = Configuration.NtfConnectionString;
                _sessionData.User = User.User2; //todo parametric
                _sessionData.OpcStatus = OpcConnectionStatus.Unknown;
                _sessionData.SlomStatus = SlomConnectionStatus.Unknown;
                _emClient = new EventMonitorClient("BasicHttpBinding_IEventMonitor", _sessionData.EmConnectionString);
                _ntfClient = new NonTransactionalFunctionsClient("BasicHttpBinding_INonTransactionalFunctions", _sessionData.NtfConnectionString);
                Initialize();
            }
            catch (Exception e)
            {
                FaultState = e;
                DispatchException("critical error when initializing the Dataservice, please restart the application", e);
            }
        }

        private readonly ManualResetEvent _systemInfoWaiter = new ManualResetEvent(false);
        private readonly ManualResetEvent _racksWaiter = new ManualResetEvent(false);
        private readonly ManualResetEvent _armsWaiter = new ManualResetEvent(false);
        private readonly ManualResetEvent _tanksWaiter = new ManualResetEvent(false);
        private readonly ManualResetEvent _sapTanksWaiter = new ManualResetEvent(false);
        private readonly ManualResetEvent _containersWaiter = new ManualResetEvent(false);
        private readonly ManualResetEvent _materialsWaiter = new ManualResetEvent(false);
        public Exception FaultState { get; private set; }

        private void Initialize()
        {
            //todo exception handling
            Task.Factory.StartNew(() =>
            {
                _ntfClient.GetSystemInfoCompleted += (sender, args) =>
                {
                    CheckAndRethrow(args.Error);
                    _systemInfo = ConvertSystemInfo(args.Result);
                    _systemInfoWaiter.Set();
                };
                _ntfClient.GetSystemInfoAsync();
                _racks = new List<Rack>{
                    new Rack{RackId = 0, RackName = "None", RackStatus = 0},
                    new Rack{RackId = 1, RackName = "North", RackStatus = 0},
                    new Rack{RackId = 2, RackName = "South", RackStatus = 0},
                    new Rack{RackId = 3, RackName = "East", RackStatus = 0}
                };
                _racksWaiter.Set();
                _ntfClient.getLoadRackArmsCompleted += (sender, args) =>
                {
                    CheckAndRethrow(args.Error);
                    _arms = ConvertArms(args.Result);
                    _armsWaiter.Set();
                };
                _ntfClient.getLoadRackArmsAsync();
                _ntfClient.GetMaterialsCompleted += (sender, args) =>
                {
                    CheckAndRethrow(args.Error);
                    _materials = ConvertMaterials(args.Result);
                    _materialsWaiter.Set();
                    _ntfClient.GetWinblendTanksCompleted += (sender2, args2) =>
                    {
                        CheckAndRethrow(args.Error);
                        _tanks = ConvertTanks(args2.Result);
                        _tanksWaiter.Set();
                    };
                    _ntfClient.GetWinblendTanksAsync();
                    _ntfClient.GetSapTanksCompleted += (sender2, args2) =>
                    {
                        CheckAndRethrow(args.Error);
                        _sapTanks = ConvertTanks(args2.Result);
                        _sapTanksWaiter.Set();
                    };
                    _ntfClient.GetSapTanksAsync();
                };
                _ntfClient.GetMaterialsAsync();
                var page = 0;
                var range = 500;
                _containers = new List<Container>();
                _ntfClient.GetContainersCompleted += (sender, args) =>
                {
                    CheckAndRethrow(args.Error);
                    _containers.AddRange(ConvertContainers(args.Result));
                    page++;
                    if (args.Result.Count >= range)
                        _ntfClient.GetContainersAsync("", page, range);
                    else
                        _containersWaiter.Set();
                };
                _ntfClient.GetContainersAsync("", page, range);
                WaitforInit();
                Messenger.Default.Send(new LoadingCompleteMessage());
            });
        }

        public void GetSessionData(Action<SessionData, Exception> callback)
        {
            callback(_sessionData, null);
        }

        public void GetSystemData(Action<SystemInfo, Exception> callback)
        {
            Task.Factory.StartNew(() =>
            {
                WaitforInit();
                callback(_systemInfo, null);
            });
        }

        public void GetRacks(Action<IList<Rack>, Exception> callback)
        {
            Task.Factory.StartNew(() =>
            {
                WaitforInit();
                callback(_racks, null);
            });
        }

        public void GetArms(Action<IList<Arm>, Exception> callback)
        {
            Task.Factory.StartNew(() =>
            {
                WaitforInit();
                callback(_arms, null);
            });
        }

        public void GetMaterials(Action<IList<Material>, Exception> callback)
        {
            Task.Factory.StartNew(() =>
            {
                WaitforInit();
                callback(_materials, null);
            });
        }

        public void GetTanks(Action<IList<Tank>, Exception> callback)
        {
            Task.Factory.StartNew(() =>
            {
                WaitforInit();
                callback(_tanks, null);
            });
        }

        public void GetSapTanks(Action<IList<Tank>, Exception> callback)
        {
            Task.Factory.StartNew(() =>
            {
                WaitforInit();
                callback(_sapTanks, null);
            });
        }

        public void GetContainers(Action<IList<Container>, Exception> callback)
        {
            Task.Factory.StartNew(() =>
            {
                WaitforInit();
                callback(_containers, null);
            });
        }

        private readonly AutoResetEvent _opcStatusLock = new AutoResetEvent(true);
        public void GetServerStatus(Action<OpcConnectionStatus, SlomConnectionStatus, Exception> callback)
        {
            var status = OpcConnectionStatus.Unknown;
            SendIt<GetOpcServerStateCompletedEventArgs>(_opcStatusLock,
                _emClient.GetOpcServerStateAsync,
                handler => _emClient.GetOpcServerStateCompleted += handler,
                handler => _emClient.GetOpcServerStateCompleted -= handler,
                args => status = ConvertOpcConnectionStatus(args.Result),
                () => callback(status, SlomConnectionStatus.Connected, null),
                e => callback(OpcConnectionStatus.Unknown, SlomConnectionStatus.Disconnected, e));
        }

        private readonly AutoResetEvent _compartmentsLock = new AutoResetEvent(true);
        public void GetCompartments(long containerId, Action<IList<Compartment>, Exception> callback)
        {
            IList<Compartment> comps = null;
            SendIt<getContainerCompartmentsCompletedEventArgs>(_compartmentsLock,
                () => _ntfClient.getContainerCompartmentsAsync(containerId),
                handler => _ntfClient.getContainerCompartmentsCompleted += handler,
                handler => _ntfClient.getContainerCompartmentsCompleted -= handler,
                args => comps = ConvertCompartments(args.Result),
                () => callback(comps,null),
                e => callback(null,e));
        }

        private readonly AutoResetEvent _ordersLock = new AutoResetEvent(true);
        public void GetOrders(Action<IList<Order>, Exception> callback)
        {
            IList<Order> obj = null;
            SendIt<GetLoadOrdersCompletedEventArgs>(_ordersLock,
                _ntfClient.GetLoadOrdersAsync,
                handler => _ntfClient.GetLoadOrdersCompleted += handler,
                handler => _ntfClient.GetLoadOrdersCompleted -= handler,
                args => obj = ConvertOrders(args.Result),
                () => callback(obj, null),
                e => callback(null, e));
        }

        private readonly AutoResetEvent _orderDetailsLock = new AutoResetEvent(true);
        public void GetOrderDetails(long orderId, Action<IList<OrderProduct>, IList<OrderCompartment>, IList<Compartment>, Container, Exception> callback)
        {
            Task.Factory.StartNew(() =>
            {
                IList<OrderProduct> orderProds = null;
                IList<OrderCompartment> orderComps = null;
                IList<Compartment> compartments = null;
                Container container = null;
                SendIt<GetLoadOrderProductsCompletedEventArgs>(_orderDetailsLock,
                    () => _ntfClient.GetLoadOrderProductsAsync(orderId),
                    handler => _ntfClient.GetLoadOrderProductsCompleted += handler,
                    handler => _ntfClient.GetLoadOrderProductsCompleted -= handler,
                    prodArgs => orderProds = ConvertOrderProducts(prodArgs.Result),
                    () => SendIt<getLoadOrderDetailsCompartmentsCompletedEventArgs>(_orderDetailsLock,
                        () => _ntfClient.getLoadOrderDetailsCompartmentsAsync(orderId),
                        handler => _ntfClient.getLoadOrderDetailsCompartmentsCompleted += handler,
                        handler => _ntfClient.getLoadOrderDetailsCompartmentsCompleted -= handler,
                        compArgs =>
                        {
                            if (compArgs.Result.Count == 0)
                                return;
                            var containerId = compArgs.Result.First().ContainerId;
                            GetCompartments(containerId, (compsList, exception) =>
                            {
                                compartments = compsList;
                                orderComps = ConvertOrderCompartments(compArgs.Result, compartments, orderProds);
                                container = compsList.FirstOrDefault().Container;
                            });
                        },
                        () => callback(orderProds, orderComps, compartments, container, null),
                        e => callback(null, null, null, null, e)
                        ),
                    e => callback(null, null, null, null, e)
                    );
            });
        }

        public void CreateOrder(Action<Exception> callback)
        {
            throw new NotImplementedException();
        }

        private static void CheckAndRethrow(Exception exception)
        {
            if (exception != null)
                throw exception;
        }

        private void DispatchException(string messageString, Exception exception)
        {
            Messenger.Default.Send(new ErrorMessage(exception, messageString));
        }

        private void SendIt<T>(AutoResetEvent locker, Action asyncAction, Action<EventHandler<T>> subscribe, Action<EventHandler<T>> unsubscribe,
    Action<T> completedAction, Action returnCallback, Action<Exception> errorCallback) where T : AsyncCompletedEventArgs
        {
            Task.Factory.StartNew(() =>
            {
                EventHandler<T> handler = null;
                Exception ex = null;
                try
                {
                    WaitforInit();
                    if (locker != null)
                    {
                        locker.WaitOne();
                    }
                    var waiter = new AutoResetEvent(false);
                    handler = (sender, args) =>
                    {
                        try
                        {
                            CheckAndRethrow(args.Error);
                            completedAction(args);
                        }
                        catch (Exception e)
                        {
                            ex = e;
                        }
                        finally
                        {
                            waiter.Set();
                        }
                    };
                    subscribe(handler);
                    asyncAction();
                    waiter.WaitOne();
                    CheckAndRethrow(ex);
                }
                catch (Exception e)
                {
                    errorCallback(e);
                }
                finally
                {
                    if (locker != null)
                        locker.Set();
                    if (handler != null)
                        unsubscribe(handler);
                }
                returnCallback();
            });
        }


        private void WaitforInit()
        {
            _systemInfoWaiter.WaitOne();
            _racksWaiter.WaitOne();
            _armsWaiter.WaitOne();
            _tanksWaiter.WaitOne();
            _sapTanksWaiter.WaitOne();
            _containersWaiter.WaitOne();
            _materialsWaiter.WaitOne();
        }

    }
}
// ReSharper restore ImplicitlyCapturedClosure