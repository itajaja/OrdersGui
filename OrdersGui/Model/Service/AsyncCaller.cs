using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Hylasoft.OrdersGui.Model.Service
{
    /// <summary>
    /// Class that exposes method to easily handle asynchronous event-based communication
    /// </summary>
    /// <typeparam name="T">Type of the return arguments, subclass of AsyncCompletedEventArgs</typeparam>
    public class AsyncCaller<T> where T : AsyncCompletedEventArgs
    {
        /// <summary>
        /// Creates an Async Caller
        /// </summary>
        /// <param name="subscribe">The method to subscribe the event handler (e.g: 'handler => FooCompleted += handler')</param>
        public AsyncCaller(Action<EventHandler<T>> subscribe)
        {
            if (subscribe == null)
                throw new ArgumentNullException("subscribe");
            subscribe(Handler);
        }

        /// <summary>
        /// Executes the call. The method returns when after receiving a response
        /// </summary>
        /// <param name="asyncAction">The asynchronous call method</param>
        /// <param name="processCallback">The callback to manipulate the data</param>
        /// <param name="callback">The callback to execute at the end of the call</param>
        public void Execute(Action asyncAction, Action<T> processCallback = null, Action<Exception> callback = null)
        {
            try
            {
                if (asyncAction == null)
                    throw new ArgumentNullException("asyncAction");
                processCallback = processCallback ?? (_ => { });
                callback = callback ?? (_ => { });
                _locker.WaitOne();
                asyncAction();
                _waiter.WaitOne();
                _locker.Set();
                CheckAndRethrow(_ex);
                processCallback(_args);
            }
            finally
            {
                callback(_ex);
            }
        }

        /// <summary>
        /// Executes the asynchronous call. The method returns after invoking the call, without waiting for the answer
        /// </summary>
        /// <param name="asyncAction">The asynchronous call method</param>
        /// <param name="processCallback">The callback to manipulate the data</param>
        /// <param name="callback">The callback to execute at the end of the call</param>
        /// <returns>A manual ResetEvent that becomes set after receiving a response</returns>
        public ManualResetEvent ExecuteAsync(Action asyncAction, Action<T> processCallback = null, Action<Exception> callback = null)
        {
            var ev = new ManualResetEvent(false);
            Task.Factory.StartNew(() =>
            {
                Execute(asyncAction, processCallback, callback);
                ev.Set();
            });
            return ev;
        }

        private void Handler(object sender, T args)
        {
            _ex = args.Error;
            _args = args;
            _waiter.Set();
        }

        private static void CheckAndRethrow(Exception exception)
        {
            if (exception != null)
                throw exception;
        }

        private readonly AutoResetEvent _waiter = new AutoResetEvent(false);
        private readonly AutoResetEvent _locker = new AutoResetEvent(true);
        private Exception _ex;
        private T _args;
    }
}