using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Net.Wifi;
using Java.Util;
using Microsoft.Maui.ApplicationModel;
using MauiLicApp.Licensing;
using Microsoft.Maui.Devices;
using System;
using System.Collections.Generic;

#pragma warning disable CS0618 // Type or member is obsolete
namespace MauiLicApp.Platforms.Android
{
    public class SystemInformation : SystemInformationBase
    {
        private Context _context => Platform.CurrentActivity ?? global::Android.App.Application.Context;

        public override string DeviceId 
        { 
            get
            {
                try
                {
                    return Settings.Secure.GetString(_context.ContentResolver, Settings.Secure.AndroidId) ?? string.Empty;
                }
                catch (System.Exception)
                {
                    return string.Empty;
                }
            }
        }
        
        public override string DeviceManufacturer => Build.Manufacturer ?? string.Empty;
        
        public override string DeviceModel => Build.Model ?? string.Empty;
        
        public override string OsVersion => Build.VERSION.Release ?? string.Empty;
        
        public override string AppVersion => AppInfo.Current?.VersionString ?? string.Empty;
        
        public override string AppBuildNumber => AppInfo.Current?.BuildString ?? string.Empty;
        
        public override string OperatingSystem => "Android";
        
        public override string SystemName => Build.Device ?? string.Empty;
        
        public override string SystemUuid => DeviceId;
        
        public override string SystemManufacturer => Build.Manufacturer ?? string.Empty;
        
        public override string SystemProductName => Build.Product ?? string.Empty;
        
        public override long TotalPhysicalMemory
        {
            get
            {
                try
                {
                    var activityManager = _context.GetSystemService(Context.ActivityService) as ActivityManager;
                    if (activityManager == null)
                        return 0;
                    
                    var memoryInfo = new ActivityManager.MemoryInfo();
                    activityManager.GetMemoryInfo(memoryInfo);
                    return memoryInfo.TotalMem;
                }
                catch
                {
                    return 0;
                }
            }
        }
        
        public override long CurrentMemoryUsage
        {
            get
            {
                try
                {
                    var activityManager = _context.GetSystemService(Context.ActivityService) as ActivityManager;
                    if (activityManager == null) 
                        return 0;
                    
                    var memoryInfo = new ActivityManager.MemoryInfo();
                    activityManager.GetMemoryInfo(memoryInfo);
                    return memoryInfo.TotalMem - memoryInfo.AvailMem;
                }
                catch
                {
                    return 0;
                }
            }
        }
        
        public override int ProcessorCount
        {
            get
            {
                try
                {
                    var runtime = Java.Lang.Runtime.GetRuntime();
                    return runtime?.AvailableProcessors() ?? System.Environment.ProcessorCount;
                }
                catch
                {
                    return System.Environment.ProcessorCount;
                }
            }
        }
        
        public override int LogicalProcessorCount
        {
            get
            {
                try
                {
                    var runtime = Java.Lang.Runtime.GetRuntime();
                    return runtime?.AvailableProcessors() ?? System.Environment.ProcessorCount;
                }
                catch
                {
                    return System.Environment.ProcessorCount;
                }
            }
        }
        
        public override string ProcessorId => Build.Hardware ?? string.Empty;
        
        public override string ProcessorName => Build.Hardware ?? string.Empty;
        
        public override string ProcessorType => Build.Hardware ?? string.Empty;
        
        public override string ProcessorArchitecture
        {
            get
            {
                try
                {
                    return Build.Supported64BitAbis?.Count > 0 ? "ARM64" : "ARM";
                }
                catch
                {
                    return "Unknown";
                }
            }
        }
        
        public override string[] ProcessorFeatures => new string[] { Build.Hardware ?? string.Empty };
        
        public override string[] EthernetCards => GetNetworkInterfaces();
        
        public override string[] DiskSerialNumbers => new string[0];
        
        public override string MotherboardSerialNumber => string.Empty;
        
        public override string BiosSerialNumber => Build.Fingerprint ?? string.Empty;
        
        public override IDictionary<string, object> DisplayProperties
        {
            get
            {
                try
                {
                    var metrics = _context?.Resources?.DisplayMetrics;
                    if (metrics == null) return new Dictionary<string, object>();
                    
                    return new Dictionary<string, object>
                    {
                        { "Width", metrics.WidthPixels },
                        { "Height", metrics.HeightPixels },
                        { "Density", metrics.Density },
                        { "DensityDpi", metrics.DensityDpi }
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
                // Check Android version to use the appropriate API
                // Use legacy API for older Android versions
                if (Build.VERSION.SdkInt < BuildVersionCodes.S)
                    return GetNetworkInterfacesLegacy();

                // For Android 12+, we cannot directly access MAC addresses
                return new string[0];
            }
            catch
            {
                return new string[0];
            }
        }
        
#pragma warning disable CA1422 // Validate platform compatibility
        private string[] GetNetworkInterfacesLegacy()
        {
            var wifiManager = _context.GetSystemService(Context.WifiService) as WifiManager;
            if (wifiManager == null) 
                return new string[0];

            var wifiInfo = wifiManager.ConnectionInfo;
            var macAddress = wifiInfo?.MacAddress ?? string.Empty;
            return new string[] { macAddress };
        }
#pragma warning restore CA1422 // Validate platform compatibility

    }
}
#pragma warning restore CS0618 // Type or member is obsolete
