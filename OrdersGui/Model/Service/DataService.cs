using System;
using System.Collections.Generic;
using System.Threading;
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
            GetSystemData((a, b) => { });
            GetRacks((a, b) => GetArms((c, d) => { }));
            GetMaterials((a,b) => GetTanks((c, d) => GetSapTanks((e, f) => { })));
//            GetContainers((a, b) => GetCompartments((c, d) => { }));
        }

        public void GetSessionData(Action<SessionData, Exception> callback)
        {
            callback(_sessionData, null);
        }

        public void GetSystemData(Action<SystemInfo, Exception> callback)
        {
            if (_systemInfo != null)
            {
                callback(_systemInfo, null);
                return;
            }
            _ntfClient.GetSystemInfoCompleted += (sender, args) =>
            {
                try
                {
                    CheckAndRethrow(args.Error);
                    _systemInfo = ConvertSystemInfo(args.Result);
                    callback(_systemInfo, null);
                }
                catch
                (Exception e)
                {
                    callback(null, e);
                }
            };
            _ntfClient.GetSystemInfoAsync();
        }

        public void GetRacks(Action<IList<Rack>, Exception> callback)
        {
            if (_racks != null)
            {
                callback(_racks, null);
                return;
            }
            _racks = new List<Rack>{
                new Rack{RackId = 0, RackName = "None", RackStatus = 0},
                new Rack{RackId = 1, RackName = "North", RackStatus = 0},
                new Rack{RackId = 2, RackName = "South", RackStatus = 0},
                new Rack{RackId = 3, RackName = "East", RackStatus = 0}
            };
            callback(_racks, null);
        }

        public void GetArms(Action<IList<Arm>, Exception> callback)
        {
            if (_arms != null)
            {
                callback(_arms, null);
                return;
            }
            _ntfClient.getLoadRackArmsCompleted += (sender, args) =>
            {
                try
                {
                    CheckAndRethrow(args.Error);
                    _arms = ConvertArms(args.Result);
                    callback(_arms, null);
                }
                catch
                (Exception e)
                {
                    callback(null, e);
                }
            };
            _ntfClient.getLoadRackArmsAsync();
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
            if (_materials != null)
            {
                callback(_materials, null);
                return;
            }
            _ntfClient.GetMaterialsCompleted += (sender, args) =>
            {
                try
                {
                    CheckAndRethrow(args.Error);
                    _materials = ConvertMaterials(args.Result);
                    callback(_materials, null);
                }
                catch
                (Exception e)
                {
                    callback(null, e);
                }
            };
            _ntfClient.GetMaterialsAsync();
        }

        public void GetTanks(Action<IList<Tank>, Exception> callback)
        {
            if (_tanks != null)
            {
                callback(_tanks, null);
                return;
            }
            _ntfClient.GetWinblendTanksCompleted += (sender, args) =>
            {
                try
                {
                    CheckAndRethrow(args.Error);
                    _tanks = ConvertTanks(args.Result);
                    callback(_tanks, null);
                }
                catch
                (Exception e)
                {
                    callback(null, e);
                }
            };
            _ntfClient.GetWinblendTanksAsync();
        }

        public void GetSapTanks(Action<IList<Tank>, Exception> callback)
        {
            if (_sapTanks != null)
            {
                callback(_sapTanks, null);
                return;
            }
            _ntfClient.GetSapTanksCompleted += (sender, args) =>
            {
                try
                {
                    CheckAndRethrow(args.Error);
                    _sapTanks = ConvertTanks(args.Result);
                    callback(_sapTanks, null);
                }
                catch
                (Exception e)
                {
                    callback(null, e);
                }
            };
            _ntfClient.GetSapTanksAsync();
        }

        public void GetCompartments(Action<IList<Compartment>, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void GetContainers(Action<IList<Container>, Exception> callback)
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