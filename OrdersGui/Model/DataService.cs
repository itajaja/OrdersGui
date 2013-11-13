using System;

namespace Hylasoft.OrdersGui.Model
{
    public class DataService : IDataService
    {
        private readonly SessionData _sessionData = new SessionData();

        public DataService()
        {
            //todo initialize sessiondata, start timers, etc
        }

        public void GetSessionData(Action<SessionData, Exception> callback)
        {
            callback(_sessionData, null);
        }
    }
}