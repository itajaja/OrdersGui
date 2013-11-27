using System;
using System.Collections.Generic;
using System.Linq;
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
    public sealed class AssignCompartmentsVM : ViewModelBase
    {
        private readonly IDataService _dataservice;
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
            set
            {
                RaiseCompletions();
                Set("OrderCompartments", ref _orderCompartments, value);
                if (_orderCompartments != null)
                    _orderCompartments.Callback = RaiseCompletions;
            }
        }

        private void RaiseCompletions()
        {
            RaisePropertyChanged("Completions");
        }


        private TrulyObservableCollection<OrderProduct> _orderProducts;
        public TrulyObservableCollection<OrderProduct> OrderProducts
        {
            get { return _orderProducts; }
            set { Set("OrderProducts", ref _orderProducts, value); }
        }

        private IList<Compartment> _compartments;
        public IList<Compartment> Compartments
        {
            get { return _compartments; }
            set
            {
                Set("Compartments", ref _compartments, value);
                RefreshCommands();
            }
        }

        private string _errorString;
        public string ErrorString
        {
            get { return _errorString; }
            set { Set("ErrorString", ref _errorString, value); }
        }

        private Container _container;
        public Container Container
        {
            get { return _container; }
            set
            {
                Set("Container", ref _container, value);
                Compartments = null;
                if (_container != null)
                    _dataservice.GetCompartments(_container.ContainerId, (list, exception) => DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        Compartments = new TrulyObservableCollection<Compartment>(list);
                    }));
                RefreshCommands();
            }
        }

        private IList<Tank> _tanks;
        public IList<Tank> Tanks
        {
            get { return _tanks; }
            set { Set("Tanks", ref _tanks, value); }
        }

        private IList<Arm> _arms;
        public IList<Arm> Arms
        {
            get { return _arms; }
            set { Set("Arms", ref _arms, value); }
        }

        public RelayCommand GoBackCommand { get; private set; }

        public IDictionary<OrderProduct, double> Completions
        {
            get
            {
                return OrderProducts != null ? OrderProducts.ToDictionary(p => p, Completion) : null;
            }
        }  

        /// <summary>
        /// Returns how much of the quota has been inserted for a product in %
        /// </summary>
        /// <param name="p">the selected product</param>
        /// <returns>the percentage of the inserted amount</returns>
        private double Completion(OrderProduct p)
        {
            return (1 - (AmountLeft(p) / p.TargetQty)) * 100;
        }

        /// <summary>
        /// Returns the amount left to insert to reach the target quantity
        /// </summary>
        /// <param name="p">the selected product</param>
        /// <returns>The amount left</returns>
        private double AmountLeft(OrderProduct p)
        {
            if (OrderCompartments == null)
                return p.TargetQty;
            var current = OrderCompartments.Where(c => c.OrderProduct == p).Sum(c => c.TargetQty);
            return p.TargetQty - current;
        }

        public AssignCompartmentsVM(IDataService ds, LoadOrderDetailsVM lodVM)
        {
            _dataservice = ds;
            ds.GetTanks((list, exception) => Tanks = list);
            Messenger.Default.Register<GoToAcMessage>(this, message =>
            {
                if (message.GoBack)
                    return;
                Order = lodVM.Order.Clone();
                ds.GetArms((arms, exception) => DispatcherHelper.CheckBeginInvokeOnUI(() =>  Arms = arms.Where(a => a.Rack == Order.LoadRack).ToList()));
                Compartments = lodVM.Compartments;
                if (lodVM.OrderProducts != null)
                    OrderProducts = lodVM.OrderProducts;
                if (lodVM.Container != null)
                    Container = lodVM.Container.Clone();
                OrderCompartments = CreateCompartments(lodVM.OrderCompartments);
                _cachedMode = lodVM.Mode;
                lodVM.Mode = DetailMode.View;
            });
            Messenger.Default.Register<UpdateQtyMessage>(this, message =>
            {
                var orderComp = message.OrderComp;
                if (orderComp == null || orderComp.OrderProduct == null || orderComp.Compartment == null)
                    return;
                orderComp.TargetQty = 0;
                orderComp.TargetQty = Math.Min(orderComp.Compartment.Capacity, AmountLeft(orderComp.OrderProduct));
            });
            Messenger.Default.Register<MoveCompMessage>(this, message =>
            {
                var to = message.OrderComp;
                var from = OrderCompartments.FirstOrDefault(c => to != c && c.Compartment == to.Compartment);
                if (from != null)
                    PutInto(from, to.SeqNo, OrderCompartments, true);

            });
            GoBackCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new GoToAcMessage(true));
                lodVM.Mode = _cachedMode;
                Reset();
            });
            Reset();
        }

        private TrulyObservableCollection<OrderCompartment> CreateCompartments(IEnumerable<OrderCompartment> originalComps)
        {
            var comps = new TrulyObservableCollection<OrderCompartment>{
                new OrderCompartment{SeqNo = SequenceNumber.A},
                new OrderCompartment{SeqNo = SequenceNumber.B},
                new OrderCompartment{SeqNo = SequenceNumber.C},
                new OrderCompartment{SeqNo = SequenceNumber.D},
                new OrderCompartment{SeqNo = SequenceNumber.E}
            };
            if (originalComps == null)
                return comps;
            var orderedComps = originalComps.OrderBy(compartment => compartment.SeqNo).ToList();
            foreach (var orderComp in orderedComps)
                PutInto(orderComp.Clone(), orderComp.SeqNo, comps);
            return comps;
        }

        private void PutInto(OrderCompartment orderComp, SequenceNumber seqNo, IList<OrderCompartment> comps, bool move = false)
        {
            var replace = comps.FirstOrDefault(c => c.SeqNo == seqNo);
            int to = comps.IndexOf(replace);
            int from = comps.IndexOf(orderComp);
            if (replace == null)
                return;
            if (move && from != -1)
                comps[from] = new OrderCompartment { SeqNo = orderComp.SeqNo };
            comps[to] = orderComp;
            orderComp.SeqNo = seqNo;
        }

        private void RefreshCommands()
        {
        }

        public void Reset()
        {
            Order = null;
            OrderCompartments = null;
            OrderProducts = null;
            Compartments = null;
            Container = null;
            Arms = null;
        }
    }

    public class UpdateQtyMessage
    {
        public OrderCompartment OrderComp { get; set; }
    }

    public class MoveCompMessage
    {
        public OrderCompartment OrderComp { get; set; }
    }
}