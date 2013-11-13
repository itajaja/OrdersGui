using System;
using Hylasoft.OrdersGui.Model;

namespace Hylasoft.OrdersGui.Design
{
    public class DesignDataService : IDataService
    {
        public void GetSessionData(Action<SessionData, Exception> callback)
        {
            // Use this to create design time data
            var data = new SessionData();
            data.ConnectionString = "localhost";
            data.OpcStatus = ConnectionStatus.Connected;
            data.SlomStatus = ConnectionStatus.Disconnected;
            data.User = User.User0;
            callback(data, null);
        }
    }
}