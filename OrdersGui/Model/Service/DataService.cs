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
        //        private readonly IList<Tank> _tanks;
        //        private readonly IList<Material> _materials; 
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
            GetSystemData((info, exception) =>
                GetRacks((a,b) =>
                    GetArms((c, d) => { })));
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

        private void CheckAndRethrow(Exception exception)
        {
            if (exception != null)
            {
                throw exception;
            }
        }
    }
}