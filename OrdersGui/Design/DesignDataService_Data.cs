using System;
using System.Collections.Generic;
using System.Linq;
using Hylasoft.OrdersGui.Model;

namespace Hylasoft.OrdersGui.Design
{
    public partial class DesignDataService
    {
        private readonly IList<Rack> _racks;
        private readonly IList<Arm> _arms;
        private readonly IList<Order> _orders;
        private readonly SystemInfo _data;
        private readonly IList<Tank> _tanks;
        private readonly IList<Material> _materials;

        public DesignDataService()
        {
            _data = new SystemInfo
            {
                ProjectTitleLine1 = @"ExxonMobil",
                ProjectTitleLine2 = @"Cicero, Illinois",
                LogoPath = @"\\WBS7_SERVER\Release\bin\@Logo.bmp",
                DateFormat = @"dd/MM/yyy hh:mm:ss",
                TimeFormat = @"24H",
                SomLoggingLevel = 0,
                SomPollingFreq = 10,
                SomMeasurementType = 0,
                SomMassUnit = @"LB",
                SomVolumeUnit = @"GAL",
                SapiaMode = 1,
                SimaticBatchVer = @"8.0",
                LegacyOrder = 0,
                SftpEnabled = false,
                SftpHost = @"134.146.113.37",
                SftpUsername = @"S.Tsttianjin-X",
                SftpInbox = @"/INT_DS_SFTP_GSAP_FMC_Inbound/",
                SftpArchive = @"/Archive/",
                SbArchivePath = @"O:\Batch Archives",
                Wb7ArchivePath = @"O:\SAPIA Archives",
                PlantName = @"Cicero",
                SomViscosityUom = @"cSt",
                SomViscTempUom = @"?C",
                AutoPrint = false,
                SomTempUnit = @"C",
                SomDensityUnit = @"kg/L",
                PlantFileName = @"ZHI",
                BatchApiVersion = @"7",
                MaxTruckWeight = 800000
            };

            _materials = new List<Material>{
                new Material{CategoryName = "",MaterialCategory = 1,MaterialCode = "43398E",MaterialFamily = 2,MaterialId = 1,MaterialName = "CASE,1GALLON,SILVER,"},
                new Material{CategoryName = "",MaterialCategory = 1,MaterialCode = "98550M",MaterialFamily = 2,MaterialId = 2,MaterialName = "SAP Order No.  22006"},
                new Material{CategoryName = "",MaterialCategory = 1,MaterialCode = "98CA64",MaterialFamily = 2,MaterialId = 3,MaterialName = "SAP Order No.  21982"},
                new Material{CategoryName = "",MaterialCategory = 2,MaterialCode = "98HC47",MaterialFamily = 1,MaterialId = 4,MaterialName = "CLEAN HIGH MILEAGE 1"},
                new Material{CategoryName = "",MaterialCategory = 2,MaterialCode = "98HG86",MaterialFamily = 1,MaterialId = 5,MaterialName = "GM GOODWRENCH SYN BL"},
                new Material{CategoryName = "",MaterialCategory = 2,MaterialCode = "98HG87",MaterialFamily = 1,MaterialId = 6,MaterialName = "GM GOODWRENCH SYN BL"},
                new Material{CategoryName = "",MaterialCategory = 3,MaterialCode = "98HH21",MaterialFamily = 1,MaterialId = 7,MaterialName = "GM GOODWRENCH SYN BL"},
                new Material{CategoryName = "",MaterialCategory = 4,MaterialCode = "98HH22",MaterialFamily = 1,MaterialId = 8,MaterialName = "GM GOODWRENCH SYN BL"},
            };

            _tanks = new List<Tank>{
                new Tank{Material = _materials[0],ApiGravity = 7,AvailabilityStatus = true,TankName = "0",TankId = 8},
                new Tank{Material = _materials[1],ApiGravity = 7,AvailabilityStatus = true,TankName = "1",TankId = 1},
                new Tank{Material = _materials[2],ApiGravity = 7,AvailabilityStatus = true,TankName = "2",TankId = 2},
                new Tank{Material = _materials[3],ApiGravity = 7,AvailabilityStatus = false,TankName = "3",TankId = 3},
                new Tank{Material = _materials[4],ApiGravity = 7,AvailabilityStatus = false,TankName = "4",TankId = 4},
                new Tank{Material = _materials[5],ApiGravity = 7,AvailabilityStatus = false,TankName = "5",TankId = 5},
                new Tank{Material = _materials[6],ApiGravity = 7,AvailabilityStatus = true,TankName = "6",TankId = 6},
                new Tank{Material = _materials[7],ApiGravity = 7,AvailabilityStatus = true,TankName = "7",TankId = 7}
            };

            _racks = new List<Rack>{
                new Rack{RackId = 0, RackName = "None", RackStatus = 0},
                new Rack{RackId = 1, RackName = "North", RackStatus = 0},
                new Rack{RackId = 2, RackName = "South", RackStatus = 0},
                new Rack{RackId = 3, RackName = "East", RackStatus = 0}
            };

            _arms = new List<Arm>{
                new Arm{ArmId = 0, ArmName = "None", ArmNo = 0, MaterialFamily = 0, Rack = _racks.First(o => o.RackId == 0)},
                new Arm{ArmId = 1, ArmName = "N1 - MOBIL1", ArmNo = 1, MaterialFamily = 1, Rack = _racks.First(o => o.RackId == 1)},
                new Arm{ArmId = 2, ArmName = "N2 - DTE", ArmNo = 2, MaterialFamily = 2, Rack = _racks.First(o => o.RackId == 1)},
                new Arm{ArmId = 3, ArmName = "N3 - ATF", ArmNo = 3, MaterialFamily = 4, Rack = _racks.First(o => o.RackId == 1)},
                new Arm{ArmId = 4, ArmName = "N4 - CORR", ArmNo = 4, MaterialFamily = 5, Rack = _racks.First(o => o.RackId == 1)},
                new Arm{ArmId = 5, ArmName = "N5 - I/M", ArmNo = 5, MaterialFamily = 3, Rack = _racks.First(o => o.RackId == 1)},
                new Arm{ArmId = 6, ArmName = "N6 - ZCF", ArmNo = 6, MaterialFamily = 10, Rack = _racks.First(o => o.RackId == 1)},
                new Arm{ArmId = 7, ArmName = "S1 - ZCF", ArmNo = 7, MaterialFamily = 10, Rack = _racks.First(o => o.RackId == 2)},
                new Arm{ArmId = 8, ArmName = "S2 - DTE", ArmNo = 8, MaterialFamily = 2, Rack = _racks.First(o => o.RackId == 2)},
                new Arm{ArmId = 9, ArmName = "S3 - DSF", ArmNo = 9, MaterialFamily = 9, Rack = _racks.First(o => o.RackId == 2)},
                new Arm{ArmId = 10, ArmName = "S4 - GRD", ArmNo = 10, MaterialFamily = 8, Rack = _racks.First(o => o.RackId == 2)},
                new Arm{ArmId = 11, ArmName = "S5 - I/M", ArmNo = 11, MaterialFamily = 3, Rack = _racks.First(o => o.RackId == 2)},
                new Arm{ArmId = 12, ArmName = "S6 - DK", ArmNo = 12, MaterialFamily = 6, Rack = _racks.First(o => o.RackId == 2)},
                new Arm{ArmId = 13, ArmName = "None", ArmNo = 13, MaterialFamily = 0, Rack = _racks.First(o => o.RackId == 3)},
                new Arm{ArmId = 14, ArmName = "None", ArmNo = 14, MaterialFamily = 0, Rack = _racks.First(o => o.RackId == 1)},
                new Arm{ArmId = 15, ArmName = "None", ArmNo = 15, MaterialFamily = 0, Rack = _racks.First(o => o.RackId == 2)},
            };

            _orders = new List<Order>{
                new Order{CarrierInstruction = "BULK ONLY:   REQUEST 5:00 A.M. PICK UPPACKAGE ONLY:1.  PALLETIZE AND", CarrierName = "None", CustomerName = "PETROLIANCE LLC", CustomerNo = "0000107805", EndDate = DateTime.Now, LoadRack = _racks.First(o => o.RackId == 0), MethodOfDelivery = DeliveryMethod.Rail, Note = "lorem ipsum", OrderId = 70065999, OrderKey = "0100514777", OrderNo = "1007151922", OrderStatus = OrderStatus.Aborted, OrderType = OrderType.Load, PoNumber = "", ScheduleDate = DateTime.Now, StartDate = DateTime.Now, TruckNo = "N9"},
                new Order{CarrierInstruction = "COFA MUST BE FAXED TO CATERPILLAR INC - PEORIA   FAX #", CarrierName = "None", CustomerName = "LOZIER OIL CO", CustomerNo = "0000100642", EndDate = DateTime.Now, LoadRack = _racks.First(o => o.RackId == 2), MethodOfDelivery = DeliveryMethod.Rail, Note = "lorem ipsum", OrderId = 70066000, OrderKey = "0100515060", OrderNo = "1007129702", OrderStatus = OrderStatus.Approved, OrderType = OrderType.Load, PoNumber = "", ScheduleDate = DateTime.Now, StartDate = DateTime.Now, TruckNo = "24"},
                new Order{CarrierInstruction = " ", CarrierName = "None", CustomerName = "VALLEY DISTRIBUTION", CustomerNo = "0000100564", EndDate = DateTime.Now, LoadRack = _racks.First(o => o.RackId == 1), MethodOfDelivery = DeliveryMethod.Rail, Note = "lorem ipsum", OrderId = 70066001, OrderKey = "0100514938", OrderNo = "1007132388", OrderStatus = OrderStatus.Cancelled, OrderType = OrderType.Load, PoNumber = "", ScheduleDate = DateTime.Now, StartDate = DateTime.Now, TruckNo = "40"},
                new Order{CarrierInstruction = "DELIVERY APPOINTMENT REQUIRED - ALL LTL DELIVERIES MUST OCCUR BETWEEN", CarrierName = "None", CustomerName = "VESCO OIL CORPORATION", CustomerNo = "0000100388", EndDate = DateTime.Now, LoadRack = _racks.First(o => o.RackId == 2), MethodOfDelivery = DeliveryMethod.Undefined, Note = "lorem ipsum", OrderId = 70066002, OrderKey = "0100515061", OrderNo = "1007014885", OrderStatus = OrderStatus.Completed, OrderType = OrderType.Load, PoNumber = "", ScheduleDate = DateTime.Now, StartDate = DateTime.Now, TruckNo = "2586"},
                new Order{CarrierInstruction = " ", CarrierName = "MARK SEVEN (VII)", CustomerName = "None", CustomerNo = "0", EndDate = DateTime.Now, LoadRack = _racks.First(o => o.RackId == 0), MethodOfDelivery = DeliveryMethod.Rail, Note = "lorem ipsum", OrderId = 70066003, OrderKey = "1413077", OrderNo = "4801587736", OrderStatus = OrderStatus.InspectionFailed, OrderType = OrderType.Unload, PoNumber = "", ScheduleDate = DateTime.Now, StartDate = DateTime.Now, TruckNo = " "},
                new Order{CarrierInstruction = " ", CarrierName = "MARK SEVEN (VII)", CustomerName = "None", CustomerNo = "0", EndDate = DateTime.Now, LoadRack = _racks.First(o => o.RackId == 1), MethodOfDelivery = DeliveryMethod.Road, Note = "lorem ipsum", OrderId = 70066004, OrderKey = "1413076", OrderNo = "4900031099", OrderStatus = OrderStatus.PendingApproval, OrderType = OrderType.Load, PoNumber = "", ScheduleDate = DateTime.Now, StartDate = DateTime.Now, TruckNo = " "},
                new Order{CarrierInstruction = " ", CarrierName = "MARK SEVEN (VII)", CustomerName = "None", CustomerNo = "0", EndDate = DateTime.Now, LoadRack = _racks.First(o => o.RackId == 0), MethodOfDelivery = DeliveryMethod.Rail, Note = "lorem ipsum", OrderId = 70066005, OrderKey = "1413079", OrderNo = "4801587738", OrderStatus = OrderStatus.Ready, OrderType = OrderType.Load, PoNumber = "", ScheduleDate = DateTime.Now, StartDate = DateTime.Now, TruckNo = " "},
                new Order{CarrierInstruction = " ", CarrierName = "MARK SEVEN (VII)", CustomerName = "None", CustomerNo = "0", EndDate = DateTime.Now, LoadRack = _racks.First(o => o.RackId == 2), MethodOfDelivery = DeliveryMethod.Undefined, Note = "lorem ipsum", OrderId = 70066006, OrderKey = "1413078", OrderNo = "4801587737", OrderStatus = OrderStatus.ReadyForRelease, OrderType = OrderType.Unload, PoNumber = "", ScheduleDate = DateTime.Now, StartDate = DateTime.Now, TruckNo = " "},
                new Order{CarrierInstruction = "BULK - SEND C OF A WITH DRIVER.PKG & DRUMS - PLEASE PALLETIZE ALL.LOAD", CarrierName = "None", CustomerName = "MORGAN DISTRIBUTING INC", CustomerNo = "0000103016", EndDate = DateTime.Now, LoadRack = _racks.First(o => o.RackId == 1), MethodOfDelivery = DeliveryMethod.Road, Note = "lorem ipsum", OrderId = 70066007, OrderKey = "0100515064", OrderNo = "1007080300", OrderStatus = OrderStatus.Released, OrderType = OrderType.Unload, PoNumber = "", ScheduleDate = DateTime.Now, StartDate = DateTime.Now, TruckNo = "02"},
                new Order{CarrierInstruction = " ", CarrierName = "APOLIS TRANSPORT INC", CustomerName = "NOCO DISTRIBUTION LLC", CustomerNo = "0000100428", EndDate = DateTime.Now, LoadRack = _racks.First(o => o.RackId == 0), MethodOfDelivery = DeliveryMethod.Rail, Note = "lorem ipsum", OrderId = 70066008, OrderKey = "0100515067", OrderNo = "1007144782", OrderStatus = OrderStatus.Ready, OrderType = OrderType.Load, PoNumber = "", ScheduleDate = DateTime.Now, StartDate = DateTime.Now, TruckNo = "325"},
                new Order{CarrierInstruction = "BULK - SEND C OF A WITH DRIVER.PKG & DRUMS - PLEASE PALLETIZE ALL.LOAD", CarrierName = "None", CustomerName = "MORGAN DISTRIBUTING INC", CustomerNo = "0000103016", EndDate = DateTime.Now, MethodOfDelivery = DeliveryMethod.Rail, Note = "lorem ipsum", OrderId = 70066009, OrderKey = "0100515066", OrderNo = "1007129894", OrderStatus = OrderStatus.Ready, OrderType = OrderType.Load, PoNumber = "", ScheduleDate = DateTime.Now, StartDate = DateTime.Now, TruckNo = "30"},
                new Order{CarrierInstruction = "PLEASE CONTACT Albert Krueger WITH PICK UP INFORMATION", CarrierName = "APOLIS TRANSPORT INC", CustomerName = "TEXAS ENTERPRISES INC", CustomerNo = "0000102068", EndDate = DateTime.Now, LoadRack = _racks.First(o => o.RackId == 2), MethodOfDelivery = DeliveryMethod.Undefined, Note = "lorem ipsum", OrderId = 70066010, OrderKey = "0100515154", OrderNo = "1007127640", OrderStatus = OrderStatus.Stopped, OrderType = OrderType.Load, PoNumber = "", ScheduleDate = DateTime.Now, StartDate = DateTime.Now, TruckNo = "50"},
                new Order{CarrierInstruction = "DELIVER 11/06/2013", CarrierName = "APOLIS TRANSPORT INC", CustomerName = "PANHANDLE EASTERN PIPE LINE,LP", CustomerNo = "0000105874", EndDate = DateTime.Now, MethodOfDelivery = DeliveryMethod.Road, Note = "lorem ipsum", OrderId = 70066011, OrderKey = "0100515152", OrderNo = "1007051267", OrderStatus = OrderStatus.Suspended, OrderType = OrderType.Load, PoNumber = "", ScheduleDate = DateTime.Now, StartDate = DateTime.Now, TruckNo = "21"},
                new Order{CarrierInstruction = "DELIVER 11/11/2013", CarrierName = "APOLIS TRANSPORT INC", CustomerName = "PANHANDLE EASTERN PIPE LINE,LP", CustomerNo = "0000105874", EndDate = DateTime.Now, LoadRack = _racks.First(o => o.RackId == 0), MethodOfDelivery = DeliveryMethod.Rail, Note = "lorem ipsum", OrderId = 70066012, OrderKey = "0100515150", OrderNo = "1006917322", OrderStatus = OrderStatus.UpdateDone, OrderType = OrderType.Load, PoNumber = "", ScheduleDate = DateTime.Now, StartDate = DateTime.Now, TruckNo = "671"},
                new Order{CarrierInstruction = "EXEL - PLEASE USE APOLIS TRANSPORT FOR CARRIER.ALL PRODUCT TO BE", CarrierName = "APOLIS TRANSPORT INC", CustomerName = "MACALLISTER MACHINERY", CustomerNo = "0000374562", EndDate = DateTime.Now, LoadRack = _racks.First(o => o.RackId == 3), MethodOfDelivery = DeliveryMethod.Road, Note = "lorem ipsum", OrderId = 70066013, OrderKey = "0100515068", OrderNo = "1007080619", OrderStatus = OrderStatus.WaitingForSeals, OrderType = OrderType.Unload, PoNumber = "", ScheduleDate = DateTime.Now, StartDate = DateTime.Now, TruckNo = "49"},
                new Order{CarrierInstruction = " ", CarrierName = "None", CustomerName = "None", CustomerNo = "0", EndDate = DateTime.Now, LoadRack = _racks.First(o => o.RackId == 0), MethodOfDelivery = DeliveryMethod.Rail, Note = "lorem ipsum", OrderId = 70066014, OrderKey = "4900031115", OrderNo = "4900031115", OrderStatus = OrderStatus.Running, OrderType = OrderType.Unload, PoNumber = "", ScheduleDate = DateTime.Now, StartDate = DateTime.Now, TruckNo = " "},
                new Order{CarrierInstruction = " ", CarrierName = " ", CustomerName = " ", CustomerNo = " ", EndDate = DateTime.Now, MethodOfDelivery = DeliveryMethod.Rail, Note = "lorem ipsum", OrderId = 70066015, OrderKey = " ", OrderNo = "123aaa", OrderStatus = OrderStatus.Ready, OrderType = OrderType.Load, PoNumber = "", ScheduleDate = DateTime.Now, StartDate = DateTime.Now, TruckNo = " "}
            };
        }
    }
}
