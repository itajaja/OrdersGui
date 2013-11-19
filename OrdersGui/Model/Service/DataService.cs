using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hylasoft.OrdersGui.EventMonitor;
using Hylasoft.OrdersGui.NonTransactionalFunctions;
using Hylasoft.OrdersGui.Resources;

namespace Hylasoft.OrdersGui.Model.Service
{
    public partial class DataService : IDataService
    {
        private readonly EventMonitorClient _emClient;
        private readonly NonTransactionalFunctionsClient _ntfClient;

        // These property are stored into the dataService because they don't need to be fetched more than once
        private readonly SessionData _sessionData = new SessionData();
        private SystemInfo _systemInfo;
        private IList<Rack> _racks;
        private IList<Arm> _arms;
        private IList<Material> _materials;
        private IList<Tank> _sapTanks;
        private IList<Tank> _tanks;
        //        private readonly IList<Container> _containers; 
        //        private readonly IList<Compartment> _compartments;

        public DataService()
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

        private readonly AutoResetEvent _systemInfoWaiter = new AutoResetEvent(false);
        private readonly AutoResetEvent _racksWaiter = new AutoResetEvent(false);
        private readonly AutoResetEvent _armsWaiter = new AutoResetEvent(false);
        private readonly AutoResetEvent _tanksWaiter = new AutoResetEvent(false);
        private readonly AutoResetEvent _sapTanksWaiter = new AutoResetEvent(false);
        private readonly AutoResetEvent _containersWaiter = new AutoResetEvent(false);
        private readonly AutoResetEvent _compartmentsWaiter = new AutoResetEvent(false);
        private readonly AutoResetEvent _materialsWaiter = new AutoResetEvent(false);
        private void Initialize()
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
                new Rack{RackId = 3, RackName = "East", RackStatus = 0}};
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
            //            GetContainers((a, b) => GetCompartments((c, d) => { }));
        }

        public void GetSessionData(Action<SessionData, Exception> callback)
        {
            callback(_sessionData, null);
        }

        public void GetSystemData(Action<SystemInfo, Exception> callback)
        {
            Task.Factory.StartNew(() =>
            {
                _systemInfoWaiter.WaitOne();
                callback(_systemInfo, null);
            });
        }

        public void GetRacks(Action<IList<Rack>, Exception> callback)
        {
            Task.Factory.StartNew(() =>
            {
                _racksWaiter.WaitOne();
                callback(_racks, null);
            });
        }

        public void GetArms(Action<IList<Arm>, Exception> callback)
        {
            Task.Factory.StartNew(() =>
            {
                _systemInfoWaiter.WaitOne();
                callback(_arms, null);
            });
        }

        public void GetOrders(Action<IList<Order>, Exception> callback)
        {
            EventHandler<GetLoadOrdersCompletedEventArgs> handler = null;
            handler = (sender, args) =>
            {
                try
                {
                    _ntfClient.GetLoadOrdersCompleted -= handler;
                    CheckAndRethrow(args.Error);
                    IList<Order> orders = ConvertOrders(args.Result);
                    callback(orders, null);
                }
                catch (Exception e)
                {
                    callback(null, e);
                }
            };
            _ntfClient.GetLoadOrdersCompleted += handler;
            _ntfClient.GetLoadOrdersAsync();
        }

        public void GetOpcStatus(Action<OpcConnectionStatus, Exception> callback)
        {
            EventHandler<GetOpcServerStateCompletedEventArgs> handler = null;
            handler = (sender, args) =>
            {
                try
                {
                    _emClient.GetOpcServerStateCompleted -= handler;
                    CheckAndRethrow(args.Error);
                    OpcConnectionStatus status = ConvertOpcConnectionStatus(args.Result);
                    callback(status, null);
                }
                catch (Exception e)
                {
                    callback(OpcConnectionStatus.Unknown, e);
                }
            };
            _emClient.GetOpcServerStateCompleted += handler;
            _emClient.GetOpcServerStateAsync();
        }

        public void GetMaterials(Action<IList<Material>, Exception> callback)
        {
            Task.Factory.StartNew(() =>
            {
                _materialsWaiter.WaitOne();
                callback(_materials, null);
            });
        }

        public void GetTanks(Action<IList<Tank>, Exception> callback)
        {
            Task.Factory.StartNew(() =>
            {
                _tanksWaiter.WaitOne();
                callback(_tanks, null);
            });
        }

        public void GetSapTanks(Action<IList<Tank>, Exception> callback)
        {
            Task.Factory.StartNew(() =>
            {
                _sapTanksWaiter.WaitOne();
                callback(_sapTanks, null);
            });
        }

        public void GetCompartments(Action<IList<Compartment>, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void GetContainers(Action<IList<Container>, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void CreateOrder(Action<Exception> callback)
        {
            throw new NotImplementedException();
        }

        private static void CheckAndRethrow(Exception exception)
        {
            if (exception != null)
            {
                throw exception;
            }
        }
    }
}