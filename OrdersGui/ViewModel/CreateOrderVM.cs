using GalaSoft.MvvmLight;
using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.ViewModel
{
    public class CreateOrderVM : ViewModelBase
    {
        private readonly IDataService _dataService;

        public CreateOrderVM(IDataService ds)
        {
            _dataService = ds;
        }
    }
}