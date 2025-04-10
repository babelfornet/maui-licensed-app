using Foundation;
using ObjCRuntime;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UIKit;
using MauiLicApp.Licensing;
using Microsoft.Maui.Devices;

namespace MauiLicApp.Platforms.iOS
{
    public class SystemInformation : SystemInformationBase
    {
        public override string DeviceId 
        { 
            get
            {
                try
                {
                    return UIDevice.CurrentDevice?.IdentifierForVendor?.AsString() ?? string.Empty;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }
        
        public override string DeviceManufacturer => "Apple";
        
        public override string DeviceModel
        {
            get
            {
                try
                {
                    return UIDevice.CurrentDevice?.Model ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        
        public override string OsVersion
        {
            get
            {
                try
                {
                    return UIDevice.CurrentDevice?.SystemVersion ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        
        public override string AppVersion
        {
            get
            {
                try
                {
                    return AppInfo.Current?.VersionString ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        
        public override string AppBuildNumber
        {
            get
            {
                try
                {
                    return AppInfo.Current?.BuildString ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        
        public override string OperatingSystem
        {
            get
            {
                try
                {
                    return UIDevice.CurrentDevice?.SystemName ?? "iOS";
                }
                catch
                {
                    return "iOS";
                }
            }
        }
        
        public override string SystemName
        {
            get
            {
                try
                {
                    return UIDevice.CurrentDevice?.Name ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        
        public override string SystemUuid
        {
            get
            {
                try
                {
                    return UIDevice.CurrentDevice?.IdentifierForVendor?.AsString() ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        
        public override string SystemManufacturer => "Apple";
        
        public override string SystemProductName
        {
            get
            {
                try
                {
                    return UIDevice.CurrentDevice?.Model ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        
        public override long TotalPhysicalMemory
        {
            get
            {
                // iOS does not provide a direct way to get total physical memory
                return 0;
            }
        }
        
        public override long CurrentMemoryUsage
        {
            get
            {
                try
                {
                    return GetUsedMemory();
                }
                catch
                {
                    return 0;
                }
            }
        }
        
        public override int ProcessorCount => System.Environment.ProcessorCount;
        
        public override int LogicalProcessorCount => System.Environment.ProcessorCount;
        
        public override string ProcessorId => "Apple";
        
        public override string ProcessorName => "Apple";
        
        public override string ProcessorType
        {
            get
            {
                try
                {
                    return Runtime.Arch.ToString();
                }
                catch
                {
                    return "Unknown";
                }
            }
        }
        
        public override string ProcessorArchitecture
        {
            get
            {
                try
                {
                    return Runtime.Arch == Arch.DEVICE ? "ARM64" : "x64";
                }
                catch
                {
                    return "Unknown";
                }
            }
        }
        
        public override string[] ProcessorFeatures
        {
            get
            {
                try
                {
                    return new string[] { Runtime.Arch.ToString() };
                }
                catch
                {
                    return new string[] { "Unknown" };
                }
            }
        }
        
        public override string[] EthernetCards => GetNetworkInterfaces();
        
        public override string[] DiskSerialNumbers
        {
            get
            {
                try
                {
                    return new string[] { UIDevice.CurrentDevice?.IdentifierForVendor?.AsString() ?? string.Empty };
                }
                catch
                {
                    return new string[] { string.Empty };
                }
            }
        }
        
        public override string MotherboardSerialNumber
        {
            get
            {
                try
                {
                    return UIDevice.CurrentDevice?.IdentifierForVendor?.AsString() ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        
        public override string BiosSerialNumber
        {
            get
            {
                try
                {
                    return UIDevice.CurrentDevice?.IdentifierForVendor?.AsString() ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        
        public override IDictionary<string, object> DisplayProperties
        {
            get
            {
                try
                {
                    var mainScreen = UIScreen.MainScreen;
                    if (mainScreen == null) return new Dictionary<string, object>();
                    
                    return new Dictionary<string, object>
                    {
                        { "Width", mainScreen.Bounds.Width * mainScreen.Scale },
                        { "Height", mainScreen.Bounds.Height * mainScreen.Scale },
                        { "Scale", mainScreen.Scale },
                        { "NativeScale", mainScreen.NativeScale }
                    };
                }
                catch
                {
                    return new Dictionary<string, object>();
                }
            }
        }

        private string[] GetNetworkInterfaces()
        {
            try
            {
                // iOS restricts access to MAC addresses
                return new string[] { "Not available on iOS" };
            }
            catch
            {
                return new string[0];
            }
        }
        
        private long GetUsedMemory()
        {
            try
            {
                using var taskInfo = new NSProcessInfo();
                return (long)taskInfo.PhysicalMemory;
            }
            catch
            {
                return 0;
            }
        }
    }
}
