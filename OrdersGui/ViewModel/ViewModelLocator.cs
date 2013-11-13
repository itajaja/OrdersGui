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

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoadOrderManagerVM>();
            SimpleIoc.Default.Register<LoadOrderDetailsVM>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public LoadOrderManagerVM LoadOrderManagerVM
        {
            get { return ServiceLocator.Current.GetInstance<LoadOrderManagerVM>(); }
        }

        public LoadOrderDetailsVM LoadOrderDetailsVM
        {
            get { return ServiceLocator.Current.GetInstance<LoadOrderDetailsVM>(); }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}