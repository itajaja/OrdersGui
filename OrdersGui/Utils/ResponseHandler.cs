using System;
using GalaSoft.MvvmLight.Threading;

namespace Hylasoft.OrdersGui.Utils
{
    public class ResponseHandler
    {
        private Action<Exception> _errorHandler;
        public Action<Exception> ErrorHandler
        {
            get { return _errorHandler ?? (e => { }); }
            set { _errorHandler = value; }
        }

        public void HandleResponse(Exception error, Action action)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                if (error != null)
                    _errorHandler(error);
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    _errorHandler(ex);
                }
            });
        }
    }
}
