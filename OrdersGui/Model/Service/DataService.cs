﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Hylasoft.OrdersGui.Utils;
using em = Hylasoft.OrdersGui.EventMonitor;
using Hylasoft.OrdersGui.Messages;
using ntf = Hylasoft.OrdersGui.NonTransactionalFunctions;
using tf = Hylasoft.OrdersGui.TransactionalFunctions;
using Hylasoft.OrdersGui.Resources;

// ReSharper disable ImplicitlyCapturedClosure
namespace Hylasoft.OrdersGui.Model.Service
{
    public partial class DataService : IDataService
    {
        private readonly em.EventMonitorClient _emClient;
        private readonly ntf.NonTransactionalFunctionsClient _ntfClient;
        private readonly tf.TransactionalFunctionsClient _tfClient;

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
                Messenger.Default.Send(new StartLoadingMessage("Connecting...", false));
                _sessionData = new SessionData();
                _sessionData.EmConnectionString = Configuration.EmConnectionString;
                _sessionData.NtfConnectionString = Configuration.NtfConnectionString;
                _sessionData.TfConnectionString = Configuration.TfConnectionString;
                _sessionData.User = User.User2; //todo parametric
                _sessionData.OpcStatus = OpcConnectionStatus.Unknown;
                _sessionData.SlomStatus = SlomConnectionStatus.Unknown;
                _emClient = new em.EventMonitorClient("BasicHttpBinding_IEventMonitor", _sessionData.EmConnectionString);
                _ntfClient = new ntf.NonTransactionalFunctionsClient("BasicHttpBinding_INonTransactionalFunctions", _sessionData.NtfConnectionString);
                _tfClient = new tf.TransactionalFunctionsClient("BasicHttpBinding_ITransactionalFunctions", _sessionData.TfConnectionString);
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

        private bool _errorState;
        private void Initialize()
        {
            _errorState = false;
            var handler = new ResponseHandler
            {
                RunOnUi = false,
                ErrorHandler = exception => DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    if (_errorState)
                        return;
                    _errorState = true;
                    var result = MessageBox.Show("A communication error occurred while initializing the communication, Do you want to retry?", "Error", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                        Initialize();
                })
            };
            Task.Factory.StartNew(() =>
            {
                _systemInfoWaiter.Reset();
                _racksWaiter.Reset();
                _armsWaiter.Reset();
                _tanksWaiter.Reset();
                _sapTanksWaiter.Reset();
                _containersWaiter.Reset();
                _materialsWaiter.Reset();
                Exception e = null;
                _ntfClient.GetSystemInfoCompleted += (sender, args) => handler.HandleResponse(args.Error, () =>
                {
                    e = e ?? args.Error;
                    _systemInfo = ConvertSystemInfo(args.Result);
                    _systemInfoWaiter.Set();
                });
                _ntfClient.GetSystemInfoAsync();
                _racks = new List<Rack>{
                        new Rack{RackId = 0, RackName = "None", RackStatus = 0},
                        new Rack{RackId = 1, RackName = "North", RackStatus = 0},
                        new Rack{RackId = 2, RackName = "South", RackStatus = 0},
                        new Rack{RackId = 3, RackName = "East", RackStatus = 0}
                    };
                _racksWaiter.Set();
                _ntfClient.getLoadRackArmsCompleted += (sender, args) => handler.HandleResponse(args.Error, () =>
                {
                    e = e ?? args.Error;
                    _arms = ConvertArms(args.Result);
                    _armsWaiter.Set();
                });
                _ntfClient.getLoadRackArmsAsync();
                _ntfClient.GetMaterialsCompleted += (sender, args) => handler.HandleResponse(args.Error, () =>
                {
                    e = e ?? args.Error;
                    _materials = ConvertMaterials(args.Result);
                    _materialsWaiter.Set();
                    _ntfClient.GetRebrandedProductsCompleted += (sender2, args2) => handler.HandleResponse(args.Error, () =>
                    {
                        e = e ?? args.Error;
                        var rebrandedProducts = ConvertRebrandedProducts(args2.Result);
                        foreach (var rebrandedProduct in rebrandedProducts)
                        {
                            var remove = _materials.SingleOrDefault(m => m.MaterialId == rebrandedProduct.MaterialId);
                            _materials.Remove(remove);
                            _materials.Add(new RebrandedProduct(remove) { Parent = rebrandedProduct.Parent });
                        }
                    });
                    _ntfClient.GetRebrandedProductsAsync();
                    _ntfClient.GetWinblendTanksCompleted += (sender2, args2) => handler.HandleResponse(args.Error, () =>
                    {
                        CheckAndRethrow(args.Error);
                        _tanks = ConvertTanks(args2.Result);
                        _tanksWaiter.Set();
                    });
                    _ntfClient.GetWinblendTanksAsync();
                    _ntfClient.GetSapTanksCompleted += (sender2, args2) =>
                    {
                        CheckAndRethrow(args.Error);
                        _sapTanks = ConvertTanks(args2.Result);
                        _sapTanksWaiter.Set();
                    };
                    _ntfClient.GetSapTanksAsync();
                });
                _ntfClient.GetMaterialsAsync();
                var page = 0;
                const int range = 500;
                _containers = new List<Container>();
                _ntfClient.GetContainersCompleted += (sender, args) => handler.HandleResponse(args.Error, () =>
                {
                    CheckAndRethrow(args.Error);
                    _containers.AddRange(ConvertContainers(args.Result));
                    page++;
                    if (args.Result.Count >= range)
                        _ntfClient.GetContainersAsync("", page, range);
                    else
                        _containersWaiter.Set();
                });
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

        private AsyncCaller<em.GetOpcServerStateCompletedEventArgs> _serverStatusCaller;
        public void GetServerStatus(Action<OpcConnectionStatus, SlomConnectionStatus, Exception> callback)
        {
            var status = OpcConnectionStatus.Unknown;
            _serverStatusCaller.ExecuteAsync(_emClient.GetOpcServerStateAsync,
                args => status = ConvertOpcConnectionStatus(args.Result),
                e => callback(status, e != null ? SlomConnectionStatus.Disconnected : SlomConnectionStatus.Connected, e));
        }

        private AsyncCaller<ntf.getContainerCompartmentsCompletedEventArgs> _compartmentsCaller;
        public void GetCompartments(long containerId, Action<IList<Compartment>, Exception> callback)
        {
            IList<Compartment> comps = null;
            _compartmentsCaller.ExecuteAsync(() => _ntfClient.getContainerCompartmentsAsync(containerId),
                args => comps = ConvertCompartments(args.Result),
                e => callback(comps, e)
                );
        }

        private AsyncCaller<ntf.GetLoadOrdersCompletedEventArgs> _orderCaller;
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
            _orderCaller = new AsyncCaller<ntf.GetLoadOrdersCompletedEventArgs>(h => _ntfClient.GetLoadOrdersCompleted += h);
            _orderProductsCaller = new AsyncCaller<ntf.GetLoadOrderProductsCompletedEventArgs>(h => _ntfClient.GetLoadOrderProductsCompleted += h);
            _orderCompartmentsCaller = new AsyncCaller<ntf.getLoadOrderDetailsCompartmentsCompletedEventArgs>(h => _ntfClient.getLoadOrderDetailsCompartmentsCompleted += h);
            _compartmentsCaller = new AsyncCaller<ntf.getContainerCompartmentsCompletedEventArgs>(h => _ntfClient.getContainerCompartmentsCompleted += h);
            _serverStatusCaller = new AsyncCaller<em.GetOpcServerStateCompletedEventArgs>(h => _emClient.GetOpcServerStateCompleted += h);
            _createOrderCaller = new AsyncCaller<AsyncCompletedEventArgs>(h => _ntfClient.InsertLoadOrderCompleted += h);
        }

        private AsyncCaller<ntf.GetLoadOrderProductsCompletedEventArgs> _orderProductsCaller;
        private AsyncCaller<ntf.getLoadOrderDetailsCompartmentsCompletedEventArgs> _orderCompartmentsCaller;
        public void GetOrderDetails(long orderId, Action<IList<OrderProduct>, IList<OrderCompartment>, IList<Compartment>, Container, Exception> callback)
        {
            Task.Factory.StartNew(() =>
            {
                IList<OrderProduct> orderProds = null;
                IList<OrderCompartment> orderComps = null;
                IList<ntf.LoadOrderDetailsCompartments> orderCompss = null;
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

        private AsyncCaller<AsyncCompletedEventArgs> _createOrderCaller;
        public void CreateOrder(Order order, IList<OrderProduct> orderProducts, Action<Exception> callback)
        {
            var convertedOrderProduct = new ObservableCollection<ntf.LoadOrderProduct>(ConvertOrderProducts(orderProducts, order.OrderId));
            order.OrderStatus = OrderStatus.Ready;
            var convertedOrder = ConvertOrder(order);
            _createOrderCaller.ExecuteAsync(() => _ntfClient.InsertLoadOrderAsync(convertedOrder, convertedOrderProduct), callback: callback);
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