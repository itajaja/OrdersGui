using System;

namespace Hylasoft.OrdersGui.Model
{
    public class DataService : IDataService
    {
        public void GetData(Action<object, Exception> callback)
        {
            // Use this to connect to the actual data service

            var item = "Welcome to MVVM Light";
            callback(item, null);
        }
    }
}