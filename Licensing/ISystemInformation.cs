using Babel.Licensing;
using System.Collections.Generic;

namespace MauiLicApp.Licensing
{
    /// <summary>
    /// Platform-independent base implementation of ISystemInformation
    /// </summary>
    public abstract class SystemInformationBase : ISystemInformation
    {
        public abstract string DeviceId { get; }
        public abstract string DeviceManufacturer { get; }
        public abstract string DeviceModel { get; }
        public abstract string OsVersion { get; }
        public abstract string AppVersion { get; }
        public abstract string AppBuildNumber { get; }
        public abstract string OperatingSystem { get; }
        public abstract string SystemName { get; }
        public abstract string SystemUuid { get; }
        public abstract string SystemManufacturer { get; }
        public abstract string SystemProductName { get; }
        public abstract long TotalPhysicalMemory { get; }
        public abstract long CurrentMemoryUsage { get; }
        public abstract int ProcessorCount { get; }
        public abstract int LogicalProcessorCount { get; }
        public abstract string ProcessorId { get; }
        public abstract string ProcessorName { get; }
        public abstract string ProcessorType { get; }
        public abstract string ProcessorArchitecture { get; }
        public abstract string[] ProcessorFeatures { get; }
        public abstract string[] EthernetCards { get; }
        public abstract string[] DiskSerialNumbers { get; }
        public abstract string MotherboardSerialNumber { get; }
        public abstract string BiosSerialNumber { get; }
        public abstract IDictionary<string, object> DisplayProperties { get; }
    }
}
