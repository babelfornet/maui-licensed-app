using Foundation;
using ObjCRuntime;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UIKit;
using MauiLicApp.Licensing;
using Microsoft.Maui.Devices;

namespace MauiLicApp.Platforms.MacCatalyst
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
                    // Return MacCatalyst instead of iOS
                    return "MacCatalyst";
                }
                catch
                {
                    return "MacCatalyst";
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
                    return Environment.Is64BitProcess ? "ARM64/x64" : "x86";
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
                    // Check if we're running on Apple Silicon
                    bool isAppleSilicon = GetIsAppleSilicon();
                    return isAppleSilicon ? "ARM64" : "x64";
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
                    bool isAppleSilicon = GetIsAppleSilicon();
                    return new string[] { isAppleSilicon ? "ARM64" : "x64" };
                }
                catch
                {
                    return new string[] { "Unknown" };
                }
            }
        }

        [DllImport("/usr/lib/libSystem.dylib")]
        private static extern int sysctlbyname([MarshalAs(UnmanagedType.LPStr)] string property, 
                                               IntPtr output, 
                                               IntPtr oldLen, 
                                               IntPtr newp, 
                                               uint newlen);
        private bool GetIsAppleSilicon()
        {
            // This is a simple approximation - in a real app you might want to use a more reliable method
            string sysname = string.Empty;
            
            try
            {
                var buffer = new byte[256];
                var bufferHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                try
                {
                    var size = buffer.Length;
                    var sizePtr = Marshal.AllocHGlobal(sizeof(int));
                    try
                    {
                        Marshal.WriteInt32(sizePtr, size);
                        
                        if (sysctlbyname("hw.machine", bufferHandle.AddrOfPinnedObject(), sizePtr, IntPtr.Zero, 0) == 0)
                        {
                            sysname = System.Text.Encoding.UTF8.GetString(buffer, 0, Math.Min(Marshal.ReadInt32(sizePtr), buffer.Length)).TrimEnd('\0');
                        }
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(sizePtr);
                    }
                }
                finally
                {
                    bufferHandle.Free();
                }
                
                // Apple Silicon Macs typically have identifiers starting with "arm64"
                return sysname.StartsWith("arm", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
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
                // MacCatalyst restricts access to MAC addresses
                return new string[] { "Not available on MacCatalyst" };
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
