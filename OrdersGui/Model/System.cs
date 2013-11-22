using Hylasoft.OrdersGui.Model.Service;

namespace Hylasoft.OrdersGui.Model
{
    public class SystemInfo : NotifyPropertyChanged
    {
        public const string Pounds = "LB";
        public const string Gallons = "GAL";

        private string _projectTitleLine1;
        public string ProjectTitleLine1
        {
            get { return _projectTitleLine1; }
            set { SetField(ref _projectTitleLine1, value, "ProjectTitleLine1"); }
        }

        private string _projectTitleLine2;
        public string ProjectTitleLine2
        {
            get { return _projectTitleLine2; }
            set { SetField(ref _projectTitleLine2, value, "ProjectTitleLine2"); }
        }

        private string _logoPath;
        public string LogoPath
        {
            get { return _logoPath; }
            set { SetField(ref _logoPath, value, "LogoPath"); }
        }

        private string _dateFormat;
        public string DateFormat
        {
            get { return _dateFormat; }
            set { SetField(ref _dateFormat, value, "DateFormat"); }
        }

        private string _timeFormat;
        public string TimeFormat
        {
            get { return _timeFormat; }
            set { SetField(ref _timeFormat, value, "TimeFormat"); }
        }

        private int _somLoggingLevel;
        public int SomLoggingLevel
        {
            get { return _somLoggingLevel; }
            set { SetField(ref _somLoggingLevel, value, "SomLoggingLevel"); }
        }

        private int _somPollingFreq;
        public int SomPollingFreq
        {
            get { return _somPollingFreq; }
            set { SetField(ref _somPollingFreq, value, "SomPollingFreq"); }
        }

        private int _somMeasurementType;
        public int SomMeasurementType
        {
            get { return _somMeasurementType; }
            set { SetField(ref _somMeasurementType, value, "SomMeasurementType"); }
        }

        private string _somMassUnit;
        public string SomMassUnit
        {
            get { return _somMassUnit; }
            set { SetField(ref _somMassUnit, value, "SomMassUnit"); }
        }

        private string _somVolumeUnit;
        public string SomVolumeUnit
        {
            get { return _somVolumeUnit; }
            set { SetField(ref _somVolumeUnit, value, "SomVolumeUnit"); }
        }

        private int _sapiaMode;
        public int SapiaMode
        {
            get { return _sapiaMode; }
            set { SetField(ref _sapiaMode, value, "SapiaMode"); }
        }

        private string _simaticBatchVer;
        public string SimaticBatchVer
        {
            get { return _simaticBatchVer; }
            set { SetField(ref _simaticBatchVer, value, "SimaticBatchVer"); }
        }

        private int _legacyOrder;
        public int LegacyOrder
        {
            get { return _legacyOrder; }
            set { SetField(ref _legacyOrder, value, "LegacyOrder"); }
        }

        private bool _sftpEnabled;
        public bool SftpEnabled
        {
            get { return _sftpEnabled; }
            set { SetField(ref _sftpEnabled, value, "SftpEnabled"); }
        }

        private string _sftpHost;
        public string SftpHost
        {
            get { return _sftpHost; }
            set { SetField(ref _sftpHost, value, "SftpHost"); }
        }

        private string _sftpUsername;
        public string SftpUsername
        {
            get { return _sftpUsername; }
            set { SetField(ref _sftpUsername, value, "SftpUsername"); }
        }

        private string _sftpInbox;
        public string SftpInbox
        {
            get { return _sftpInbox; }
            set { SetField(ref _sftpInbox, value, "SftpInbox"); }
        }

        private string _sftpArchive;
        public string SftpArchive
        {
            get { return _sftpArchive; }
            set { SetField(ref _sftpArchive, value, "SftpArchive"); }
        }

        private string _sbArchivePath;
        public string SbArchivePath
        {
            get { return _sbArchivePath; }
            set { SetField(ref _sbArchivePath, value, "SbArchivePath"); }
        }

        private string _wb7ArchivePath;
        public string Wb7ArchivePath
        {
            get { return _wb7ArchivePath; }
            set { SetField(ref _wb7ArchivePath, value, "Wb7ArchivePath"); }
        }

        private string _plantName;
        public string PlantName
        {
            get { return _plantName; }
            set { SetField(ref _plantName, value, "PlantName"); }
        }

        private string _somViscosityUom;
        public string SomViscosityUom
        {
            get { return _somViscosityUom; }
            set { SetField(ref _somViscosityUom, value, "SomViscosityUom"); }
        }

        private string _somViscTempUom;
        public string SomViscTempUom
        {
            get { return _somViscTempUom; }
            set { SetField(ref _somViscTempUom, value, "SomViscTempUom"); }
        }

        private bool _autoPrint;
        public bool AutoPrint
        {
            get { return _autoPrint; }
            set { SetField(ref _autoPrint, value, "AutoPrint"); }
        }

        private string _somTempUnit;
        public string SomTempUnit
        {
            get { return _somTempUnit; }
            set { SetField(ref _somTempUnit, value, "SomTempUnit"); }
        }

        private string _somDensityUnit;
        public string SomDensityUnit
        {
            get { return _somDensityUnit; }
            set { SetField(ref _somDensityUnit, value, "SomDensityUnit"); }
        }

        private string _plantFileName;
        public string PlantFileName
        {
            get { return _plantFileName; }
            set { SetField(ref _plantFileName, value, "PlantFileName"); }
        }

        private string _batchApiVersion;
        public string BatchApiVersion
        {
            get { return _batchApiVersion; }
            set { SetField(ref _batchApiVersion, value, "BatchApiVersion"); }
        }

        private double _maxTruckWeight;
        public double MaxTruckWeight
        {
            get { return _maxTruckWeight; }
            set { SetField(ref _maxTruckWeight, value, "MaxTruckWeight"); }
        }
    }
}
