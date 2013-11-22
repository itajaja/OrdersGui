using System;
using System.Collections.Generic;
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
                InitCallers();
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

        private AsyncCaller<GetOpcServerStateCompletedEventArgs> _serverStatusCaller;
        public void GetServerStatus(Action<OpcConnectionStatus, SlomConnectionStatus, Exception> callback)
        {
            var status = OpcConnectionStatus.Unknown;
            _serverStatusCaller.ExecuteAsync(_emClient.GetOpcServerStateAsync,
                args => status = ConvertOpcConnectionStatus(args.Result),
                e => callback(status, e != null ? SlomConnectionStatus.Disconnected : SlomConnectionStatus.Connected, e));
        }

        private AsyncCaller<getContainerCompartmentsCompletedEventArgs> _compartmentsCaller;
        public void GetCompartments(long containerId, Action<IList<Compartment>, Exception> callback)
        {
            IList<Compartment> comps = null;
            _compartmentsCaller.ExecuteAsync(() => _ntfClient.getContainerCompartmentsAsync(containerId),
                args => comps = ConvertCompartments(args.Result),
                e => callback(comps, e)
                );
        }

        private AsyncCaller<GetLoadOrdersCompletedEventArgs> _orderCaller;
        public void GetOrders(Action<IList<Order>, Exception> callback)
        {
            IList<Order> orders = null;
            _orderCaller.ExecuteAsync(_ntfClient.GetLoadOrdersAsync,
                args =>
                {
                    orders = ConvertOrders(args.Result);
                },
                e => callback(orders, e)
                );
        }

        private void InitCallers()
        {
            _orderCaller = new AsyncCaller<GetLoadOrdersCompletedEventArgs>(h => _ntfClient.GetLoadOrdersCompleted += h);
            _orderProductsCaller = new AsyncCaller<GetLoadOrderProductsCompletedEventArgs>(h => _ntfClient.GetLoadOrderProductsCompleted += h);
            _orderCompartmentsCaller = new AsyncCaller<getLoadOrderDetailsCompartmentsCompletedEventArgs>(h => _ntfClient.getLoadOrderDetailsCompartmentsCompleted += h);
            _compartmentsCaller = new AsyncCaller<getContainerCompartmentsCompletedEventArgs>(h => _ntfClient.getContainerCompartmentsCompleted += h);
            _serverStatusCaller = new AsyncCaller<GetOpcServerStateCompletedEventArgs>(h => _emClient.GetOpcServerStateCompleted += h);
        }

        private AsyncCaller<GetLoadOrderProductsCompletedEventArgs> _orderProductsCaller;
        private AsyncCaller<getLoadOrderDetailsCompartmentsCompletedEventArgs> _orderCompartmentsCaller;
        public void GetOrderDetails(long orderId, Action<IList<OrderProduct>, IList<OrderCompartment>, IList<Compartment>, Container, Exception> callback)
        {
            Task.Factory.StartNew(() =>
            {
                IList<OrderProduct> orderProds = null;
                IList<OrderCompartment> orderComps = null;
                IList<LoadOrderDetailsCompartments> orderCompss = null;
                IList<Compartment> compartments = null;
                Container container = null;
                Exception ex = null;
                Action call = () => callback(orderProds, orderComps, compartments, container, ex);
                _orderProductsCaller.ExecuteAsync(() => _ntfClient.GetLoadOrderProductsAsync(orderId),
                    args => orderProds = ConvertOrderProducts(args.Result),
                    e => ex = e
                    ).WaitOne();
                if (ex != null)
                {
                    call();
                    return;
                }
                _orderCompartmentsCaller.ExecuteAsync(() => _ntfClient.getLoadOrderDetailsCompartmentsAsync(orderId),
                    args => orderCompss = args.Result,
                    e => ex = e
                    ).WaitOne();
                if (ex != null || orderCompss.Count == 0)
                {
                    call();
                    return;
                }
                var containerId = orderCompss.First().ContainerId;
                GetCompartments(containerId, (compsList, exception) =>
                {
                    compartments = compsList;
                    orderComps = ConvertOrderCompartments(orderCompss, compartments, orderProds);
                    container = compsList.FirstOrDefault().Container;
                    call();
                });
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