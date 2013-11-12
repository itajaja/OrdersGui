using System;
using Hylasoft.OrdersGui.Model;

namespace Hylasoft.OrdersGui.Design
{
    public class DesignDataService : IDataService
    {
        public void GetData(Action<object, Exception> callback)
        {
            // Use this to create design time data

            var item = "Welcome to MVVM Light [design]";
            callback(item, null);
        }
    }
}