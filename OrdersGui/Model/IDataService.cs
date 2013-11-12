using System;

namespace Hylasoft.OrdersGui.Model
{
    public interface IDataService
    {
        void GetData(Action<object, Exception> callback);
    }
}
