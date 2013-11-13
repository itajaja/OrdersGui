using System;

namespace Hylasoft.OrdersGui.Model
{
    public interface IDataService
    {
        void GetSessionData(Action<SessionData, Exception> callback);
    }
}
