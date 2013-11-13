﻿using System;
using System.Collections.Generic;
using Hylasoft.OrdersGui.Resources;

namespace Hylasoft.OrdersGui.Model.Service
{
    public partial class DataService : IDataService
    {
        private readonly EventMonitor.EventMonitorClient _emClient;
        private readonly NonTransactionalFunctions.NonTransactionalFunctionsClient _ntfClient;

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
            //setup connections and configurations
            _sessionData = new SessionData();
            _sessionData.ConnectionString = Configuration.ConnectionString;
            _sessionData.User = User.User2; //todo parametric
            _sessionData.OpcStatus = ConnectionStatus.Unknown;
            _sessionData.SlomStatus = ConnectionStatus.Unknown;
            _emClient = new EventMonitor.EventMonitorClient("BasicHttpBinding_IEventMonitor", _sessionData.ConnectionString);
            _ntfClient = new NonTransactionalFunctions.NonTransactionalFunctionsClient("BasicHttpBinding_INonTransactionalFunctions");
            
            //initialize racks, arms, tanks, sysInfo, etc. everything that should be initialized and supposedly not modified
            _ntfClient.GetSystemInfoCompleted += (sender, args) => { _systemInfo = ConvertSystemInfo(args.Result); };
            _ntfClient.GetSystemInfoAsync();

            //todo this should be pulled from server, the method must be created
            _racks = new List<Rack>{
                new Rack{RackId = 0, RackName = "None", RackStatus = 0},
                new Rack{RackId = 1, RackName = "North", RackStatus = 0},
                new Rack{RackId = 2, RackName = "South", RackStatus = 0},
                new Rack{RackId = 3, RackName = "East", RackStatus = 0}
            };
            
            _ntfClient.getLoadRackArmsCompleted += (sender, args) => { _arms = ConvertArms(args.Result); };
            _ntfClient.getLoadRackArmsAsync();
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
            _ntfClient.GetLoadOrdersCompleted += (sender, args) =>
            {
                IList<Order> orders = ConvertOrders(args.Result);
                callback(orders, null);
            };
            _ntfClient.GetLoadOrdersAsync();
        }
    }
}