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

        private Action<Action> _runOnUi = DispatcherHelper.CheckBeginInvokeOnUI;
        public bool RunOnUi
        {
            set
            {
                if (value)
                    _runOnUi = DispatcherHelper.CheckBeginInvokeOnUI;
                else
                    _runOnUi = action => action();
            }
        }

        public void HandleResponse(Exception error, Action action)
        {
            _runOnUi(() =>
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
