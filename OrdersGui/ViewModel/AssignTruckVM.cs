﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Hylasoft.OrdersGui.Messages;
using Hylasoft.OrdersGui.Model;
using Hylasoft.OrdersGui.Model.Service;
using Hylasoft.OrdersGui.Utils;

namespace Hylasoft.OrdersGui.ViewModel
{
    public sealed class AssignTruckVM : ViewModelBase
    {
        private readonly IDataService _dataservice;
        private SessionData _sessionData;
        private DetailMode _cachedMode;

        private Order _order;
        public Order Order
        {
            get { return _order; }
            set { Set("Order", ref _order, value); }
        }

        private TrulyObservableCollection<OrderCompartment> _orderCompartments;
        public TrulyObservableCollection<OrderCompartment> OrderCompartments
        {
            get { return _orderCompartments; }
            set { Set("OrderCompartments", ref _orderCompartments, value); }
        }

        private TrulyObservableCollection<Compartment> _compartments;
        public TrulyObservableCollection<Compartment> Compartments
        {
            get { return _compartments; }
            set { Set("Compartments", ref _compartments, value); }
        }

        private Container _container;
        public Container Container
        {
            get { return _container; }
            set
            {
                Set("Container", ref _container, value);
                if (_container != null)
                    _dataservice.GetCompartments(_container.ContainerId, (list, exception) =>DispatcherHelper.CheckBeginInvokeOnUI( () =>
                    {
                        Compartments = new TrulyObservableCollection<Compartment>(list);
                    }));
            }
        }
        
        private IList<Container> _containers;
        public IList<Container> Containers
        {
            get { return _containers; }
            set
            {
                Set("Containers", ref _containers, value);
                ContainersView = new PagedCollectionView(_containers);
                ContainersView.Filter = o =>
                    CultureInfo.CurrentCulture.CompareInfo.IndexOf(((Container)o).ContainerNo, ContainerFilter, CompareOptions.IgnoreCase) >= 0;
            }
        }

        private PagedCollectionView _containersView;
        public PagedCollectionView ContainersView
        {
            get { return _containersView; }
            set { Set("ContainersView", ref _containersView, value); }
        }

        private string _containerFilter = "";
        public string ContainerFilter
        {
            get { return _containerFilter; }
            set
            {
                Set("ContainerFilter", ref _containerFilter, value);
                if (ContainersView != null)
                    ContainersView.Refresh();
            }
        }

        public RelayCommand GoBackCommand { get; private set; }
        public RelayCommand AssignTruckCommand { get; private set; }

        public AssignTruckVM(IDataService ds, LoadOrderDetailsVM lodVM)
        {
            _dataservice = ds;
            _dataservice.GetSessionData((data, exception) => _sessionData = data);
            _dataservice.GetContainers((data, exception) => DispatcherHelper.CheckBeginInvokeOnUI(() =>Containers = data));
            Messenger.Default.Register<GoToAtMessage>(this, message =>
            {
                if (message.GoBack)
                    return;
                Order = lodVM.Order.Clone();
                Compartments = lodVM.Compartments;
                if (lodVM.Container != null)
                    Container = lodVM.Container.Clone();
                if (lodVM.OrderCompartments != null)
                    OrderCompartments = new TrulyObservableCollection<OrderCompartment>(lodVM.OrderCompartments.Select(o => o.Clone()));
                _cachedMode = lodVM.Mode;
                lodVM.Mode = DetailMode.View;
            });
            GoBackCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new GoToAtMessage(true));
                lodVM.Mode = _cachedMode;
                Reset();
            });
            AssignTruckCommand = new RelayCommand(() =>
            {
                var confirm = MessageBox.Show("Are you sure you want to change the current truck?","Confirm",MessageBoxButton.OKCancel);
                if (confirm != MessageBoxResult.OK)
                    return;
                //assign truck
            });
            Reset();
        }

        private void RefreshCommands()
        {
            AssignTruckCommand.RaiseCanExecuteChanged();
        }

        public void Reset()
        {
            Order = null;
            OrderCompartments = null;
            Compartments = null;
            Container = null;
        }
    }

}