using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
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
        private const double CompletionScale = 100;
        private const string InvalidArm = "None";
        private const string InvalidTank = "unknown";

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
                Validate();
                Set("OrderCompartments", ref _orderCompartments, value);
                if (_orderCompartments != null)
                    _orderCompartments.Callback = Validate;
            }
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

        private string _warningString;
        public string WarningString
        {
            get { return _warningString; }
            set { Set("WarningString", ref _warningString, value); }
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
        public RelayCommand AssignCompartmentCommand { get; private set; }

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
            return (1 - (AmountLeft(p) / p.TargetQty)) * CompletionScale;
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

        private void Validate()
        {
            RaisePropertyChanged("Completions");
            RefreshCommands();
        }

        private bool CanAssignCompartments()
        {
            WarningString = "";
            var warningBuilder = new StringBuilder("");
            if (Completions == null || OrderCompartments == null || Compartments == null || Order == null)
                return false;
            foreach (var item in Completions.Where(item => item.Value < CompletionScale))
            {
                ErrorString = "Quantity for Product " + item.Key.Material.MaterialName + " has to reach the target Quantity.\n" +
                              "Target Quantity: " + item.Key.TargetQty + "\n" +
                              "Current Quantity inserted: " + (item.Key.TargetQty - AmountLeft(item.Key));
                return false;
            }
            var usedOrderComps = OrderCompartments.Where(orderComp => orderComp.Compartment != null && orderComp.OrderProduct != null).ToList();
            foreach (var orderComp in usedOrderComps)
            {
                if (orderComp.TargetQty > orderComp.Compartment.Capacity)
                {
                    ErrorString = "The quantity inserted for compartment " + orderComp.Compartment.CompartmentNo + " cannot exceed its capacity.\n" +
                                  "Capacity: " + orderComp.Compartment.Capacity + "\n" +
                                  "Current Quantity inserted: " + orderComp.TargetQty;
                    return false;
                }
                if (Order.OrderType == OrderType.Load)
                {
                    if ((orderComp.RackArm == null || orderComp.RackArm.ArmName == InvalidArm))
                    {
                        ErrorString = "No arm selected for compartment No. " + orderComp.Compartment.CompartmentNo + ".";
                        return false;
                    }
                    if (orderComp.RackArm.MaterialFamily != orderComp.OrderProduct.Material.MaterialFamily)
                    {
                        warningBuilder.AppendLine(orderComp.OrderProduct.Material.MaterialName + " material family does not match arm " + orderComp.RackArm.ArmName +" material family");
                    }
                    if (orderComp.TargetQty < 0)
                    {
                        ErrorString = "The quantity set for the compartment No. " + orderComp.Compartment.CompartmentNo + "must be greater than 0.";
                        return false;
                    }
                }
                if (orderComp.Tank == null)
                {
                    ErrorString = "The selected tank for compartment No. " + orderComp.Compartment.CompartmentNo + " must be selected.";
                    return false;
                }
                if (orderComp.Tank.TankName.ToLower() == InvalidTank)
                {
                    warningBuilder.AppendLine("Tank " + InvalidTank + " selected for compartment " + orderComp.Compartment.CompartmentNo);
                }

            }
            if (!OrderCompartments.Take(usedOrderComps.Count).SequenceEqual(usedOrderComps))
            {
                ErrorString = "Cannot have holes in the sequence.";
                return false;
            }
            foreach (var c in Compartments)
            {
                if (OrderCompartments.Count(o => o.Compartment == c) > 1)
                {
                    ErrorString = "Compartment No. " + c.CompartmentNo + " is selected more than once.";
                    return false;
                }
            }
            WarningString = warningBuilder.ToString();
            ErrorString = "";
            return true;
        }

        public AssignCompartmentsVM(IDataService ds, LoadOrderDetailsVM lodVM)
        {
            _dataservice = ds;
            ds.GetTanks((list, exception) => Tanks = list);
            Messenger.Default.Register<GoToAcMessage>(this,
                message =>
                {
                    if (message.GoBack)
                        return;
                    Order = lodVM.Order.Clone();
                    ds.GetArms((arms, exception) => DispatcherHelper.CheckBeginInvokeOnUI(() => Arms = arms.Where(a => a.Rack == Order.LoadRack).ToList()));
                    Compartments = lodVM.Compartments;
                    if (lodVM.OrderProducts != null)
                        OrderProducts = new TrulyObservableCollection<OrderProduct>(lodVM.OrderProducts);
                    if (lodVM.Container != null)
                        Container = lodVM.Container.Clone();
                    OrderCompartments = CreateCompartments(lodVM.OrderCompartments);
                    _cachedMode = lodVM.Mode;
                    lodVM.Mode = DetailMode.View;
                    Validate();
                });
            Messenger.Default.Register<UpdateQtyMessage>(this,
                message =>
                {
                    var orderComp = message.OrderComp;
                    if (orderComp == null || orderComp.OrderProduct == null || orderComp.Compartment == null)
                        return;
                    orderComp.TargetQty = 0;
                    orderComp.TargetQty = Math.Min(orderComp.Compartment.Capacity, AmountLeft(orderComp.OrderProduct));
                });
            Messenger.Default.Register<MoveCompMessage>(this,
                message =>
                {
                    var to = message.OrderComp;
                    var from = OrderCompartments.FirstOrDefault(c => to != c && c.Compartment == to.Compartment);
                    if (from != null)
                        PutInto(from, to.SeqNo, OrderCompartments, true);
                });
            AssignCompartmentCommand = new RelayCommand(
                // ReSharper disable once ImplicitlyCapturedClosure
                () =>
                {
                    var dialogString = "Do you wish to confirm?";
                    if (!string.IsNullOrEmpty(WarningString))
                        dialogString = "Attention, You have warnings.\n" + dialogString;
                    MessageBoxResult result = MessageBox.Show(dialogString, "Confirm", MessageBoxButton.OKCancel);
                    if (result != MessageBoxResult.OK)
                        return;
                    lodVM.OrderProducts = OrderProducts;//todo implement call
                    //todo change order status
                    GoBackCommand.Execute(null);
                }, CanAssignCompartments);
            GoBackCommand = new RelayCommand(
                // ReSharper disable once ImplicitlyCapturedClosure
                () =>
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
            AssignCompartmentCommand.RaiseCanExecuteChanged();
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