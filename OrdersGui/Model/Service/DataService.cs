using System;
using System.Collections.Generic;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
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
            try
            {
                //setup connections and configurations
                _sessionData = new SessionData();
                _sessionData.EmConnectionString = Configuration.EmConnectionString;
                _sessionData.NtfConnectionString = Configuration.NtfConnectionString;
                _sessionData.User = User.User2; //todo parametric
                _sessionData.OpcStatus = OpcConnectionStatus.Unknown;
                _sessionData.SlomStatus = SlomConnectionStatus.Unknown;
                _emClient = new EventMonitorClient("BasicHttpBinding_IEventMonitor", _sessionData.EmConnectionString);
                _ntfClient = new NonTransactionalFunctionsClient("BasicHttpBinding_INonTransactionalFunctions", _sessionData.NtfConnectionString);

                //initialize racks, arms, tanks, sysInfo, etc. everything that should be initialized and supposedly not modified
                _ntfClient.GetSystemInfoCompleted += (sender, args) =>
                {
                    HandleError(args.Error);
                    _systemInfo = ConvertSystemInfo(args.Result);
                };
                _ntfClient.GetSystemInfoAsync();

                //todo this should be pulled from server, the method must be created
                _racks = new List<Rack>{
                    new Rack{RackId = 0, RackName = "None", RackStatus = 0},
                    new Rack{RackId = 1, RackName = "North", RackStatus = 0},
                    new Rack{RackId = 2, RackName = "South", RackStatus = 0},
                    new Rack{RackId = 3, RackName = "East", RackStatus = 0}
                };

                _ntfClient.getLoadRackArmsCompleted += (sender, args) =>
                {
                    HandleError(args.Error);
                    _arms = ConvertArms(args.Result);
                };
                _ntfClient.getLoadRackArmsAsync();
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        public void GetSessionData(Action<SessionData, Exception> callback)
        {
            callback(_sessionData, null);
        }

        public void GetSystemData(Action<SystemInfo, Exception> callback)
        {
            callback(_systemInfo, null);
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
            EventHandler<GetLoadOrdersCompletedEventArgs> handler = null;
            handler = (sender, args) =>
            {
                try
                {
                    _ntfClient.GetLoadOrdersCompleted -= handler;
                    HandleError(args.Error);
                    IList<Order> orders = ConvertOrders(args.Result);
                    callback(orders, null);
                }
                catch (Exception)
                {
                    callback(null, args.Error);
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
                    HandleError(args.Error);
                    OpcConnectionStatus status = ConvertOpcConnectionStatus(args.Result);
                    callback(status, null);
                }
                catch (Exception)
                {
                    callback(OpcConnectionStatus.Unknown, args.Error);
                }
            };
            _emClient.GetOpcServerStateCompleted += handler;
            _emClient.GetOpcServerStateAsync();
        }

        private void HandleError(Exception exception)
        {
            if (exception != null)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    _sessionData.OpcStatus = OpcConnectionStatus.Unknown;
                    _sessionData.SlomStatus = SlomConnectionStatus.Disconnected;
                });
                throw exception;
            }
        }
    }
}