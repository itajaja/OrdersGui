using GalaSoft.MvvmLight;
using Hylasoft.OrdersGui.Model;
using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        /// <summary>
        /// The <see cref="SessionData" /> property's name.
        /// </summary>
        public const string SessionDataPropertyName = "SessionData";

        private SessionData _sessionData;

        /// <summary>
        /// Sets and gets the SessionData property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public SessionData SessionData
        {
            get
            { return _sessionData;}
            set
            {
                if (_sessionData == value)
                {
                    return;
                }

                RaisePropertyChanging(SessionDataPropertyName);
                _sessionData = value;
                RaisePropertyChanged(SessionDataPropertyName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetSessionData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    SessionData = item;
                });
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}