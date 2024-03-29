﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Hylasoft.OrdersGui.Messages;
using Hylasoft.OrdersGui.Model;
using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.ViewModel
{
    public sealed class LoadOrderDetailsVM : ViewModelBase
    {
        private readonly IDataService _dataservice;

        private IList<Rack> _racks;

        private DetailMode _mode;
        public DetailMode Mode
        {
            get { return _mode; }
            set { Set("Mode", ref _mode, value); }
        }

        private Order _order;
        public Order Order
        {
            get { return _order; }
            set { Set("Order", ref _order, value); }
        }

        private IList<OrderProduct> _orderProducts;
        public IList<OrderProduct> OrderProducts
        {
            get { return _orderProducts; }
            set { Set("OrderProducts", ref _orderProducts, value); }
        }

        private IList<OrderCompartment> _orderCompartments;
        public IList<OrderCompartment> OrderCompartments
        {
            get { return _orderCompartments; }
            set { Set("OrderCompartments", ref _orderCompartments, value); }
        }

        private IList<Compartment> _compartments;
        public IList<Compartment> Compartments
        {
            get { return _compartments; }
            set { Set("Compartments", ref _compartments, value); }
        }

        private Container _container;
        public Container Container
        {
            get { return _container; }
            set { Set("Container", ref _container, value); }
        }

        private DateTime _editDate;
        public DateTime EditDate
        {
            get { return _editDate; }
            set { Set("EditDate", ref _editDate, value); }
        }

        private bool _isEditingDate;
        private DateTime _editTime;
        public DateTime EditTime
        {
            get { return _editTime; }
            set { Set("EditTime", ref _editTime, value); }
        }

        public RelayCommand GoBackCommand { get; private set; }
        public RelayCommand AssignCompartmentCommand { get; private set; }
        public RelayCommand AssignTruckCommand { get; private set; }
        public RelayCommand FulfillOrderCommand { get; private set; }
        public RelayCommand<string> ChangeRackCommand { get; private set; }
        public RelayCommand SaveDateCommand { get; private set; }
        public RelayCommand EditDateCommand { get; private set; }

        public LoadOrderDetailsVM(IDataService ds)
        {
            _dataservice = ds;
            _dataservice.GetRacks((list, exception) => _racks = list);
            Messenger.Default.Register<GoToLodMessage>(this, o =>
            {
                Mode = o.Mode;
                Order = o.Order;
                _dataservice.GetOrderDetails(Order.OrderId,
                    (orderProds, orderComps, comps, container, error) => DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        if (orderProds != null)
                            OrderProducts = new ObservableCollection<OrderProduct>(orderProds);
                        if (orderComps != null)
                        {
                            OrderCompartments = new ObservableCollection<OrderCompartment>(orderComps.OrderBy(c => c.SeqNo));
                            Compartments = new ObservableCollection<Compartment>(comps);
                            Container = container;
                        }
                    }));
            });
            GoBackCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new GoToLomMessage());
                Reset();
            });
            AssignCompartmentCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new GoToAcMessage(false));
                Messenger.Default.Send(new OpenCloseEditDateMessage(false));
            },
                () => Order != null && Container != null && Compartments != null && Order.OrderStatus != OrderStatus.Ready && Mode == DetailMode.Prepare);
            AssignTruckCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new GoToAtMessage(false));
                Messenger.Default.Send(new OpenCloseEditDateMessage(false));
            },
                () => Order != null && Mode == DetailMode.Prepare);
            FulfillOrderCommand = new RelayCommand(() => MessageBox.Show("fullfill"),
                () => Order != null && Mode == DetailMode.Fullfill);
            EditDateCommand = new RelayCommand(() =>
            {
                if (_isEditingDate)
                {
                    Messenger.Default.Send(new OpenCloseEditDateMessage(false));
                    _isEditingDate = false;
                    return;
                }
                _isEditingDate = true;
                var currDate = Order.ScheduleDate;
                EditDate = currDate;
                EditTime = currDate;
                Messenger.Default.Send(new OpenCloseEditDateMessage(true));
            },
                () => Order != null && Mode == DetailMode.Edit);
            SaveDateCommand = new RelayCommand(() =>
            {
                var newDate = new DateTime(EditDate.Year, EditDate.Month, EditDate.Day, EditTime.Hour, EditTime.Minute, 0);
                var confirm = MessageBox.Show("Are you sure to change date and time?\nFrom: " + Order.ScheduleDate + "\nTo: " + newDate, "Change Rack", MessageBoxButton.OKCancel);
                if (confirm == MessageBoxResult.OK)
                {
                    Order.ScheduleDate = newDate;
                    Messenger.Default.Send(new OpenCloseEditDateMessage(false));
                    _isEditingDate = false;
                }
            },
                () => Order != null && Mode == DetailMode.Edit);
            ChangeRackCommand = new RelayCommand<string>(s =>
            {
                var currentRack = Order.LoadRack;
                var newRack = _racks.First(r => r.RackName == s);
                if (currentRack == newRack)
                    return;
                var confirm = MessageBox.Show("Are you sure to change rack from " + currentRack.RackName + " to " + s + "?", "Change Rack", MessageBoxButton.OKCancel);
                if (confirm == MessageBoxResult.OK)
                    Order.LoadRack = newRack;
            },
                s => Order != null && !String.IsNullOrEmpty(s) && Mode != DetailMode.View && Order.OrderType == OrderType.Load && Container == null);
            var t = new DispatcherTimer();
            t.Interval = TimeSpan.FromSeconds(1);
            t.Tick += (_, __) => RefreshCommands();
            t.Start();
        }

        private void RefreshCommands()
        {
            GoBackCommand.RaiseCanExecuteChanged();
            AssignCompartmentCommand.RaiseCanExecuteChanged();
            AssignTruckCommand.RaiseCanExecuteChanged();
            FulfillOrderCommand.RaiseCanExecuteChanged();
            EditDateCommand.RaiseCanExecuteChanged();
            SaveDateCommand.RaiseCanExecuteChanged();
            ChangeRackCommand.RaiseCanExecuteChanged();
        }

        public void Reset()
        {
            Order = null;
            OrderProducts = null;
            OrderCompartments = null;
            Compartments = null;
            Container = null;
            Messenger.Default.Send(new GoToAtMessage(true));
            Messenger.Default.Send(new OpenCloseEditDateMessage(false));
        }
    }

    public class OpenCloseEditDateMessage
    {
        public bool Open { get; set; }

        public OpenCloseEditDateMessage(bool open)
        {
            Open = open;
        }
    }
}