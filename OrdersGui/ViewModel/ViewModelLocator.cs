using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Hylasoft.OrdersGui.Model.Service;
using Microsoft.Practices.ServiceLocation;

namespace Hylasoft.OrdersGui.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<LoadOrderManagerVM>(false);
            SimpleIoc.Default.Register<LoadOrderDetailsVM>(false);
            SimpleIoc.Default.Register<CreateOrderVM>(false);
            SimpleIoc.Default.Register<AssignTruckVM>(false);
            SimpleIoc.Default.Register<AssignCompartmentsVM>(false);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public LoadOrderManagerVM LoadOrderManagerVM
        {
            get { return ServiceLocator.Current.GetInstance<LoadOrderManagerVM>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public LoadOrderDetailsVM LoadOrderDetailsVM
        {
            get { return ServiceLocator.Current.GetInstance<LoadOrderDetailsVM>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public CreateOrderVM CreateOrderVM
        {
            get { return ServiceLocator.Current.GetInstance<CreateOrderVM>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public AssignTruckVM AssignTruckVM
        {
            get { return ServiceLocator.Current.GetInstance<AssignTruckVM>(); }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public AssignCompartmentsVM AssignCompartmentsVM
        {
            get { return ServiceLocator.Current.GetInstance<AssignCompartmentsVM>(); }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}