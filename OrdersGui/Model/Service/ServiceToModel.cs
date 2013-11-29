using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using nt = Hylasoft.OrdersGui.NonTransactionalFunctions;

//This file contains all the method to convert from the service types to the model types. All the Logic for this task should reside here, so that the service are not used outside this class
namespace Hylasoft.OrdersGui.Model.Service
{

    public partial class DataService
    {
        private Arm ConvertArm(nt.LoadRackArm arm)
        {
            var mArm = new Arm();
            mArm.ArmId = arm.LoadArmId;
            mArm.ArmName = arm.LoadArmName;
            mArm.ArmNo = arm.LoadArmNo;
            mArm.Rack = _racks.FirstOrDefault(a => a.RackId == arm.LoadRackId);
            mArm.MaterialFamily = arm.MaterialFamily;
            return mArm;
        }

        private IList<Arm> ConvertArms(IEnumerable<nt.LoadRackArm> arms)
        {
            return arms.Select(ConvertArm).ToList();
        }

        private SystemInfo ConvertSystemInfo(nt.Wb7System system)
        {
            var vSystem = new SystemInfo();
            vSystem.AutoPrint = system.AutoPrint;
            vSystem.BatchApiVersion = system.BatchApiVersion;
            vSystem.DateFormat = system.DateFormat;
            vSystem.LegacyOrder = system.LegacyOrder;
            vSystem.LogoPath = system.LogoPath;
            vSystem.MaxTruckWeight = system.MaxTruckWeight;
            vSystem.PlantFileName = system.PlantFileName;
            vSystem.PlantName = system.PlantName;
            vSystem.ProjectTitleLine1 = system.ProjectTitleLine1;
            vSystem.ProjectTitleLine2 = system.ProjectTitleLine2;
            vSystem.SapiaMode = system.SapiaMode;
            vSystem.SbArchivePath = system.SbArchivePath;
            vSystem.SftpArchive = system.SftpArchive;
            vSystem.SftpEnabled = system.SftpEnabled;
            vSystem.SftpHost = system.SftpHost;
            vSystem.SftpInbox = system.SftpInbox;
            vSystem.SftpUsername = system.SftpUsername;
            vSystem.SimaticBatchVer = system.SimaticBatchVer;
            vSystem.SomDensityUnit = system.SomDensityUnit;
            vSystem.SomLoggingLevel = system.SomLoggingLevel;
            vSystem.SomMassUnit = system.SomMassUnit;
            vSystem.SomMeasurementType = system.SomMeasurementType;
            vSystem.SomPollingFreq = system.SomPollingFreq;
            vSystem.SomTempUnit = system.SomTempUnit;
            vSystem.SomViscTempUom = system.SomViscTempUom;
            vSystem.SomViscosityUom = system.SomViscosityUom;
            vSystem.SomVolumeUnit = system.SomVolumeUnit;
            vSystem.TimeFormat = system.TimeFormat;
            vSystem.Wb7ArchivePath = system.Wb7ArchivePath;
            return vSystem;
        }

        private Order ConvertOrder(nt.LoadOrder order)
        {
            var mOrder = new Order();
            mOrder.CarrierInstruction = order.CarrierInstruction;
            mOrder.CarrierName = order.CarrierName;
            mOrder.CustomerName = order.CustomerName;
            mOrder.CustomerNo = order.CustomerNo;
            mOrder.EndDate = order.EndDate;
            mOrder.LoadRack = _racks.FirstOrDefault(a => a.RackId == order.LoadRackId);
            mOrder.MethodOfDelivery = ConvertDeliveryMethod(order.MethodOfDeliveryDescription);
            mOrder.Note = order.Note;
            mOrder.OrderId = order.OrderId;
            mOrder.OrderKey = order.OrderKey;
            mOrder.OrderNo = order.OrderNo;
            mOrder.OrderStatus = ConvertOrderStatus(order.OrderStatus);
            mOrder.OrderType = ConvertOrderType(order.OrderType);
            mOrder.PoNumber = order.PONumber;
            mOrder.ScheduleDate = order.ScheduleDate;
            mOrder.StartDate = order.StartDate;
            mOrder.TruckNo = order.TruckNo;
            return mOrder;
        }

        private IList<Order> ConvertOrders(IEnumerable<nt.LoadOrder> orders)
        {
            return orders.Select(ConvertOrder).ToList();
        }

        private nt.LoadOrderStatus ConvertOrderStatus(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Idle:
                    return nt.LoadOrderStatus.IDLE;
                case OrderStatus.Ready:
                    return nt.LoadOrderStatus.READY;
                case OrderStatus.TruckArrived:
                    return nt.LoadOrderStatus.TRUCK_ARRIVED;
                case OrderStatus.ReadyForRelease:
                    return nt.LoadOrderStatus.READY_FOR_RELEASE;
                case OrderStatus.ReleaseInProgress:
                    return nt.LoadOrderStatus.RELEASE_IN_PROGRESS;
                case OrderStatus.Released:
                    return nt.LoadOrderStatus.RELEASED;
                case OrderStatus.ReleaseError:
                    return nt.LoadOrderStatus.RELEASE_ERROR;
                case OrderStatus.Running:
                    return nt.LoadOrderStatus.RUNNING;
                case OrderStatus.Stopped:
                    return nt.LoadOrderStatus.STOPPED;
                case OrderStatus.Completed:
                    return nt.LoadOrderStatus.COMPLETED;
                case OrderStatus.Aborted:
                    return nt.LoadOrderStatus.ABORTED;
                case OrderStatus.Cancelled:
                    return nt.LoadOrderStatus.CANCELLED;
                case OrderStatus.Suspended:
                    return nt.LoadOrderStatus.SUSPENDED;
                case OrderStatus.Approved:
                    return nt.LoadOrderStatus.APPROVED;
                case OrderStatus.Rejected:
                    return nt.LoadOrderStatus.REJECTED;
                case OrderStatus.WaitingForSeals:
                    return nt.LoadOrderStatus.WAITING_FOR_SEALS;
                case OrderStatus.EndFilling:
                    return nt.LoadOrderStatus.END_FILLING;
                case OrderStatus.ReportReady:
                    return nt.LoadOrderStatus.REPORT_READY;
                case OrderStatus.ReportRetry:
                    return nt.LoadOrderStatus.REPORT_RETRY;
                case OrderStatus.UpdateDone:
                    return nt.LoadOrderStatus.UPDATE_DONE;
                case OrderStatus.Setup:
                    return nt.LoadOrderStatus.SETUP;
                case OrderStatus.InspectionFailed:
                    return nt.LoadOrderStatus.INSPECTION_FAILED;
                case OrderStatus.PendingApproval:
                    return nt.LoadOrderStatus.PENDING_APPROVAL;
                case OrderStatus.SapUpdateDone:
                    return nt.LoadOrderStatus.SAP_UPDATE_DONE;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private OrderStatus ConvertOrderStatus(nt.LoadOrderStatus status)
        {
            switch (status)
            {
                case nt.LoadOrderStatus.IDLE:
                    return OrderStatus.Idle;
                case nt.LoadOrderStatus.READY:
                    return OrderStatus.Ready;
                case nt.LoadOrderStatus.TRUCK_ARRIVED:
                    return OrderStatus.TruckArrived;
                case nt.LoadOrderStatus.READY_FOR_RELEASE:
                    return OrderStatus.ReadyForRelease;
                case nt.LoadOrderStatus.RELEASE_IN_PROGRESS:
                    return OrderStatus.ReleaseInProgress;
                case nt.LoadOrderStatus.RELEASED:
                    return OrderStatus.Released;
                case nt.LoadOrderStatus.RELEASE_ERROR:
                    return OrderStatus.ReleaseError;
                case nt.LoadOrderStatus.RUNNING:
                    return OrderStatus.Running;
                case nt.LoadOrderStatus.STOPPED:
                    return OrderStatus.Stopped;
                case nt.LoadOrderStatus.COMPLETED:
                    return OrderStatus.Completed;
                case nt.LoadOrderStatus.ABORTED:
                    return OrderStatus.Aborted;
                case nt.LoadOrderStatus.CANCELLED:
                    return OrderStatus.Cancelled;
                case nt.LoadOrderStatus.SUSPENDED:
                    return OrderStatus.Suspended;
                case nt.LoadOrderStatus.APPROVED:
                    return OrderStatus.Approved;
                case nt.LoadOrderStatus.REJECTED:
                    return OrderStatus.Rejected;
                case nt.LoadOrderStatus.WAITING_FOR_SEALS:
                    return OrderStatus.WaitingForSeals;
                case nt.LoadOrderStatus.END_FILLING:
                    return OrderStatus.EndFilling;
                case nt.LoadOrderStatus.REPORT_READY:
                    return OrderStatus.ReportReady;
                case nt.LoadOrderStatus.REPORT_RETRY:
                    return OrderStatus.ReportRetry;
                case nt.LoadOrderStatus.UPDATE_DONE:
                    return OrderStatus.UpdateDone;
                case nt.LoadOrderStatus.SETUP:
                    return OrderStatus.Setup;
                case nt.LoadOrderStatus.INSPECTION_FAILED:
                    return OrderStatus.InspectionFailed;
                case nt.LoadOrderStatus.PENDING_APPROVAL:
                    return OrderStatus.PendingApproval;
                case nt.LoadOrderStatus.SAP_UPDATE_DONE:
                    return OrderStatus.SapUpdateDone;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private OrderType ConvertOrderType(string type)
        {
            return (OrderType)Enum.Parse(typeof(OrderType), type, true);
        }

        private string ConvertOrderType(OrderType type)
        {
            switch (type)
            {
                case OrderType.Load:
                    return "L";
                case OrderType.Unload:
                    return "U";
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        private DeliveryMethod ConvertDeliveryMethod(string method)
        {
            return (DeliveryMethod)Enum.Parse(typeof(DeliveryMethod), method, true);
        }

        private string ConvertDeliveryMethod(DeliveryMethod method)
        {
            switch (method)
            {
                case DeliveryMethod.Road:
                    return "1";
                case DeliveryMethod.Rail:
                    return "20";
                case DeliveryMethod.Undefined:
                    return "E1";
                default:
                    throw new ArgumentOutOfRangeException("method");
            }
        }

        private OpcConnectionStatus ConvertOpcConnectionStatus(string status)
        {
            switch (status)
            {
                case "OPCRunning":
                    return OpcConnectionStatus.Running;
                case "OPCFailed":
                    return OpcConnectionStatus.Failed;
                case "OPCNoConfig":
                    return OpcConnectionStatus.NoConfiguration;
                case "OPCSuspended":
                    return OpcConnectionStatus.Suspended;
                case "OPCTest":
                    return OpcConnectionStatus.Test;
                case "OPCDisconnected":
                    return OpcConnectionStatus.Disconnected;
                default:
                    return OpcConnectionStatus.Unknown;
            }
        }

        private Material ConvertMaterial(nt.Material material)
        {
            var mMaterial = new Material();
            mMaterial.CategoryName = material.CategoryName;
            mMaterial.DataEntryValue = material.DataEntryValue;
            mMaterial.MaterialCategory = material.MaterialCategory;
            mMaterial.MaterialCode = material.MaterialCode;
            mMaterial.MaterialFamily = material.MaterialFamily;
            mMaterial.MaterialId = material.MaterialId;
            mMaterial.MaterialName = material.MaterialName;
            return mMaterial;
        }

        private IList<Material> ConvertMaterials(IEnumerable<nt.Material> orders)
        {
            return orders.Select(ConvertMaterial).ToList();
        }

        private Tank ConvertTank(nt.Tank tank)
        {
            var mTank = new Tank();
            mTank.ApiGravity = tank.APIGravity;
            mTank.AvailabilityStatus = tank.AvailabilityStatus == 1;
            mTank.Material = _materials.FirstOrDefault(m => m.MaterialId == tank.MaterialId);
            mTank.SapTankName = tank.SapTankName;
            mTank.TankId = tank.TankId;
            mTank.TankName = tank.TankName;
            return mTank;
        }

        private IList<Tank> ConvertTanks(IEnumerable<nt.Tank> orders)
        {
            return orders.Select(ConvertTank).ToList();
        }

        private Container ConvertContainer(nt.Container cont)
        {
            var mCont = new Container();
            mCont.Capacity = cont.Capacity;
            mCont.CompartmentsCount = cont.Compartments;
            mCont.ContainerId = cont.ContainerId;
            mCont.ContainerNo = cont.ContainerNo;
            mCont.GrossWeight = cont.GrossWeight;
            mCont.Type = cont.Type;
            return mCont;
        }

        private IEnumerable<Container> ConvertContainers(IEnumerable<nt.Container> containers)
        {
            return containers.Select(ConvertContainer).ToList();
        }

        private Compartment ConvertCompartment(nt.Compartment comp)
        {
            var mComp = new Compartment();
            mComp.Capacity = comp.Capacity;
            mComp.CompartmentNo = comp.CompartmentNo;
            mComp.Container = _containers.FirstOrDefault(c => c.ContainerId == comp.ContainerID);
            return mComp;
        }

        private IList<Compartment> ConvertCompartments(IEnumerable<nt.Compartment> compartments)
        {
            return compartments.Select(ConvertCompartment).ToList();
        }

        private OrderProduct ConvertOrderProduct(nt.LoadOrderProduct product)
        {
            var mProduct = new OrderProduct();
            mProduct.Material = _materials.FirstOrDefault(m => m.MaterialCode == product.MaterialCode);
            mProduct.SourceTank = _tanks.FirstOrDefault(t => t.TankName == product.SourceTank);
            if (mProduct.SourceTank != null && mProduct.Material != null) //chose a default one
                mProduct.Material.FindTanks(_tanks).FirstOrDefault();
            mProduct.TargetQty = product.TargetQty;
            mProduct.Uom = product.UoM;
            return mProduct;
        }

        private IList<OrderProduct> ConvertOrderProducts(IEnumerable<nt.LoadOrderProduct> products)
        {
            return products.Select(ConvertOrderProduct).ToList();
        }

        private LoadingCompartmentStatus? ConvertCompartmentStatus(int compartmentStatus)
        {
            switch (compartmentStatus)
            {
                case 0:return LoadingCompartmentStatus.Free;
                case 1:return LoadingCompartmentStatus.Ready;
                case 2:return LoadingCompartmentStatus.Filling;
                case 3:return LoadingCompartmentStatus.Stopped;
                case 4:return LoadingCompartmentStatus.Completed;
                case 5:return LoadingCompartmentStatus.Partial;
                case 6:return LoadingCompartmentStatus.Aborted;
                case 7:return LoadingCompartmentStatus.Unused;
                default: return null;
            }
        }

        private OrderCompartment ConvertOrderCompartment(nt.LoadOrderDetailsCompartments comp, IEnumerable<Compartment> comps = null, IEnumerable<OrderProduct> prods = null)
        {
            var mComp = new OrderCompartment();
            if (comps != null)
            {
                mComp.Compartment = comps.FirstOrDefault(c => c.CompartmentNo == comp.CompartmentNo);
                if (mComp.Compartment != null)
                    mComp.CompartmentStatus = ConvertCompartmentStatus(comp.CompartmentStatus);
            }
            if (prods != null)
                mComp.OrderProduct = prods.FirstOrDefault(p => p.Material.MaterialCode == comp.MaterialCode);
            mComp.ActualQty = comp.ActualQty;
            mComp.BatchNumber = comp.BatchNumber;
            mComp.NetWeight = comp.NetWeight;
            mComp.RackArm = _arms.FirstOrDefault(a => a.ArmId == comp.LoadArmId);
            mComp.SeqNo = (SequenceNumber)comp.SeqNo;
            mComp.Tank = _tanks.FirstOrDefault(t => t.TankId == comp.TankId);
            mComp.TargetQty = comp.TargetQty;
            return mComp;
        }

        private IList<OrderCompartment> ConvertOrderCompartments(IEnumerable<nt.LoadOrderDetailsCompartments> compartments, IEnumerable<Compartment> comps = null, IEnumerable<OrderProduct> prods = null)
        {
            return compartments.Select(c => ConvertOrderCompartment(c,comps,prods)).ToList();
        }

        private RebrandedProduct ConvertRebrandedProduct(nt.RebrandedProduct product)
        {
            var mproduct = new RebrandedProduct();
            mproduct.MaterialCode = product.MaterialCode;
            mproduct.MaterialId = product.MaterialId;
            mproduct.Parent = _materials.FirstOrDefault(m => m.MaterialCode == product.MaterialCode);
            return mproduct;
        }

        private IEnumerable<RebrandedProduct> ConvertRebrandedProducts(IEnumerable<nt.RebrandedProduct> products)
        {
            return products.Select(ConvertRebrandedProduct).ToList();
        }

        private nt.LoadOrder ConvertOrder(Order order)
        {
            var nOrder = new nt.LoadOrder();
            nOrder.CarrierInstruction = order.CarrierInstruction;
            nOrder.CarrierName = order.CarrierName;
            nOrder.CustomerName = order.CustomerName;
            nOrder.CustomerNo = order.CustomerNo;
            nOrder.EndDate = order.EndDate;
            nOrder.MethodOfDelivery = ConvertDeliveryMethod(order.MethodOfDelivery);
            nOrder.Note = order.Note;
            nOrder.OrderId = order.OrderId;
            nOrder.OrderKey = order.OrderKey;
            nOrder.OrderNo = order.OrderNo;
            nOrder.OrderStatus = ConvertOrderStatus(order.OrderStatus);
            nOrder.OrderType = ConvertOrderType(order.OrderType);
            nOrder.PONumber = order.PoNumber;
            nOrder.ScheduleDate = order.ScheduleDate;
            nOrder.StartDate = order.StartDate;
            nOrder.TruckNo = order.TruckNo;
            if (order.LoadRack != null)
            {
                nOrder.LoadRackId = order.LoadRack.RackId;
                nOrder.LoadRackName = order.LoadRack.RackName;
            }
            return nOrder;
        }

        private nt.LoadOrderProduct ConvertOrderProduct(OrderProduct product, long orderId)
        {
            var nProduct = new nt.LoadOrderProduct();
            if (product.Material != null)
            {
                nProduct.MaterialCode = product.Material.MaterialCode;
                nProduct.MaterialFamily = product.Material.MaterialFamily;
                nProduct.MaterialName = product.Material.MaterialName;
            }
            if (product.SourceTank != null)
                nProduct.SourceTank = product.SourceTank.TankName;
            nProduct.TargetQty = product.TargetQty;
            nProduct.OrderId = orderId;
            nProduct.UoM = product.Uom;
            return nProduct;
        }

        private IEnumerable<nt.LoadOrderProduct> ConvertOrderProducts(IEnumerable<OrderProduct> orderProducts, long orderId)
        {
            return orderProducts.Select(product => ConvertOrderProduct(product, orderId)).ToList();
        }
    }
}
