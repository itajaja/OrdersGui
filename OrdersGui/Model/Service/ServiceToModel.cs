using System;
using System.Collections.Generic;
using System.Linq;
using Hylasoft.OrdersGui.NonTransactionalFunctions;

//This file contains all the method to convert from the service types to the model types. All the Logic for this task should reside here, so that the service are not used outside this class
namespace Hylasoft.OrdersGui.Model.Service
{

    public partial class DataService
    {
        private Arm ConvertArm(LoadRackArm arm)
        {
            var mArm = new Arm();
            mArm.ArmId = arm.LoadArmId;
            mArm.ArmName = arm.LoadArmName;
            mArm.ArmNo = arm.LoadArmNo;
            mArm.Rack = _racks.FirstOrDefault(a => a.RackId == arm.LoadRackId);
            mArm.MaterialFamily = arm.MaterialFamily;
            return mArm;
        }

        private IList<Arm> ConvertArms(IEnumerable<LoadRackArm> arms)
        {
            return arms.Select(ConvertArm).ToList();
        }

        private SystemInfo ConvertSystemInfo(Wb7System system)
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

        private Order ConvertOrder(LoadOrder order)
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

        private IList<Order> ConvertOrders(IEnumerable<LoadOrder> orders)
        {
            return orders.Select(ConvertOrder).ToList();
        }

        private OrderStatus ConvertOrderStatus(LoadOrderStatus status)
        {
            switch (status)
            {
                case LoadOrderStatus.IDLE:
                    return OrderStatus.Idle;
                case LoadOrderStatus.READY:
                    return OrderStatus.Ready;
                case LoadOrderStatus.TRUCK_ARRIVED:
                    return OrderStatus.TruckArrived;
                case LoadOrderStatus.READY_FOR_RELEASE:
                    return OrderStatus.ReadyForRelease;
                case LoadOrderStatus.RELEASE_IN_PROGRESS:
                    return OrderStatus.ReleaseInProgress;
                case LoadOrderStatus.RELEASED:
                    return OrderStatus.Released;
                case LoadOrderStatus.RELEASE_ERROR:
                    return OrderStatus.ReleaseError;
                case LoadOrderStatus.RUNNING:
                    return OrderStatus.Running;
                case LoadOrderStatus.STOPPED:
                    return OrderStatus.Stopped;
                case LoadOrderStatus.COMPLETED:
                    return OrderStatus.Completed;
                case LoadOrderStatus.ABORTED:
                    return OrderStatus.Aborted;
                case LoadOrderStatus.CANCELLED:
                    return OrderStatus.Cancelled;
                case LoadOrderStatus.SUSPENDED:
                    return OrderStatus.Suspended;
                case LoadOrderStatus.APPROVED:
                    return OrderStatus.Approved;
                case LoadOrderStatus.REJECTED:
                    return OrderStatus.Rejected;
                case LoadOrderStatus.WAITING_FOR_SEALS:
                    return OrderStatus.WaitingForSeals;
                case LoadOrderStatus.END_FILLING:
                    return OrderStatus.EndFilling;
                case LoadOrderStatus.REPORT_READY:
                    return OrderStatus.ReportReady;
                case LoadOrderStatus.REPORT_RETRY:
                    return OrderStatus.ReportRetry;
                case LoadOrderStatus.UPDATE_DONE:
                    return OrderStatus.UpdateDone;
                case LoadOrderStatus.SETUP:
                    return OrderStatus.Setup;
                case LoadOrderStatus.INSPECTION_FAILED:
                    return OrderStatus.InspectionFailed;
                case LoadOrderStatus.PENDING_APPROVAL:
                    return OrderStatus.PendingApproval;
                case LoadOrderStatus.SAP_UPDATE_DONE:
                    return OrderStatus.SapUpdateDone;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private OrderType ConvertOrderType(string type)
        {
            return (OrderType)Enum.Parse(typeof(OrderType), type, true);
        }

        private DeliveryMethod ConvertDeliveryMethod(string method)
        {
            return (DeliveryMethod)Enum.Parse(typeof(DeliveryMethod), method, true);
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

        public void GetMaterials(Action<IList<Material>, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void GetTanks(Action<IList<Tank>, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void GetCompartments(Action<IList<Compartment>, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void GetContainers(Action<IList<Container>, Exception> callback)
        {
            throw new NotImplementedException();
        }
    }
}