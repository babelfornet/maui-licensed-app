using System;
using System.Collections.Generic;
using System.Management;
using System.Runtime.InteropServices;
using Microsoft.Maui.Devices;
using Microsoft.UI.Xaml;
using MauiLicApp.Licensing;
using Windows.System.Profile;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace MauiLicApp.Platforms.Windows
{
    public class SystemInformation : SystemInformationBase
    {
        public override string DeviceId
        {
            get
            {
                try
                {
                    var deviceInfo = new EasClientDeviceInformation();
                    return deviceInfo.Id.ToString();
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        public override string DeviceManufacturer
        {
            get
            {
                try
                {
                    var deviceInfo = new EasClientDeviceInformation();
                    return deviceInfo.SystemManufacturer;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        public override string DeviceModel
        {
            get
            {
                try
                {
                    var deviceInfo = new EasClientDeviceInformation();
                    return deviceInfo.SystemProductName;
                }
                catch (Exception)
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
                    var version = Environment.OSVersion.Version;
                    return $"{version.Major}.{version.Minor}.{version.Build}";
                }
                catch (Exception)
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
                catch (Exception)
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
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        public override string OperatingSystem => "Windows";

        public override string SystemName
        {
            get
            {
                try
                {
                    return Environment.MachineName;
                }
                catch (Exception)
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
                    using var searcher = new ManagementObjectSearcher("SELECT UUID FROM Win32_ComputerSystemProduct");
                    foreach (var obj in searcher.Get())
                    {
                        return obj["UUID"]?.ToString() ?? string.Empty;
                    }
                    return string.Empty;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        public override string SystemManufacturer
        {
            get
            {
                try
                {
                    using var searcher = new ManagementObjectSearcher("SELECT Manufacturer FROM Win32_ComputerSystem");
                    foreach (var obj in searcher.Get())
                    {
                        return obj["Manufacturer"]?.ToString() ?? string.Empty;
                    }
                    return string.Empty;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        public override string SystemProductName
        {
            get
            {
                try
                {
                    using var searcher = new ManagementObjectSearcher("SELECT Model FROM Win32_ComputerSystem");
                    foreach (var obj in searcher.Get())
                    {
                        return obj["Model"]?.ToString() ?? string.Empty;
                    }
                    return string.Empty;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        public override long TotalPhysicalMemory
        {
            get
            {
                try
                {
                    using var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
                    foreach (var obj in searcher.Get())
                    {
                        if (ulong.TryParse(obj["TotalPhysicalMemory"]?.ToString(), out var memory))
                        {
                            return (long)memory;
                        }
                    }
                    return 0;
                }
                catch (Exception)
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
                    using var searcher = new ManagementObjectSearcher("SELECT FreePhysicalMemory FROM Win32_OperatingSystem");
                    foreach (var obj in searcher.Get())
                    {
                        if (ulong.TryParse(obj["FreePhysicalMemory"]?.ToString(), out var freeMemory))
                        {
                            // FreePhysicalMemory is in KB, convert to bytes
                            var freeMemoryBytes = freeMemory * 1024;
                            return TotalPhysicalMemory - (long)freeMemoryBytes;
                        }
                    }
                    return 0;
                }
                catch (Exception)
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
                    using var searcher = new ManagementObjectSearcher("SELECT NumberOfProcessors FROM Win32_ComputerSystem");
                    foreach (var obj in searcher.Get())
                    {
                        if (int.TryParse(obj["NumberOfProcessors"]?.ToString(), out var count))
                        {
                            return count;
                        }
                    }
                    return Environment.ProcessorCount;
                }
                catch (Exception)
                {
                    return Environment.ProcessorCount;
                }
            }
        }

        public override int LogicalProcessorCount
        {
            get
            {
                try
                {
                    using var searcher = new ManagementObjectSearcher("SELECT NumberOfLogicalProcessors FROM Win32_ComputerSystem");
                    foreach (var obj in searcher.Get())
                    {
                        if (int.TryParse(obj["NumberOfLogicalProcessors"]?.ToString(), out var count))
                        {
                            return count;
                        }
                    }
                    return Environment.ProcessorCount;
                }
                catch (Exception)
                {
                    return Environment.ProcessorCount;
                }
            }
        }

        public override string ProcessorId
        {
            get
            {
                try
                {
                    using var searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor");
                    foreach (var obj in searcher.Get())
                    {
                        return obj["ProcessorId"]?.ToString() ?? string.Empty;
                    }
                    return string.Empty;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        public override string ProcessorName
        {
            get
            {
                try
                {
                    using var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor");
                    foreach (var obj in searcher.Get())
                    {
                        return obj["Name"]?.ToString() ?? string.Empty;
                    }
                    return string.Empty;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        public override string ProcessorType
        {
            get
            {
                try
                {
                    using var searcher = new ManagementObjectSearcher("SELECT Description FROM Win32_Processor");
                    foreach (var obj in searcher.Get())
                    {
                        return obj["Description"]?.ToString() ?? string.Empty;
                    }
                    return string.Empty;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        public override string ProcessorArchitecture
        {
            get
            {
                try
                {
                    if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
                        return "x64";
                    else if (RuntimeInformation.ProcessArchitecture == Architecture.X86)
                        return "x86";
                    else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                        return "ARM64";
                    else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm)
                        return "ARM";
                    else
                        return RuntimeInformation.ProcessArchitecture.ToString();
                }
                catch (Exception)
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
                    var features = new List<string>();
                    using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                    foreach (var obj in searcher.Get())
                    {
                        features.Add(obj["Name"]?.ToString() ?? string.Empty);
                        break; // Just get the first processor
                    }
                    return features.ToArray();
                }
                catch (Exception)
                {
                    return new string[] { "Unknown" };
                }
            }
        }

        public override string[] EthernetCards
        {
            get
            {
                try
                {
                    var macAddresses = new List<string>();
                    using var searcher = new ManagementObjectSearcher("SELECT MACAddress FROM Win32_NetworkAdapter WHERE PhysicalAdapter=True");
                    foreach (var obj in searcher.Get())
                    {
                        var mac = obj["MACAddress"]?.ToString();
                        if (!string.IsNullOrEmpty(mac))
                        {
                            macAddresses.Add(mac);
                        }
                    }
                    return macAddresses.ToArray();
                }
                catch (Exception)
                {
                    return new string[0];
                }
            }
        }

        public override string[] DiskSerialNumbers
        {
            get
            {
                try
                {
                    var serials = new List<string>();
                    using var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive");
                    foreach (var obj in searcher.Get())
                    {
                        var serial = obj["SerialNumber"]?.ToString();
                        if (!string.IsNullOrEmpty(serial))
                        {
                            serials.Add(serial);
                        }
                    }
                    return serials.ToArray();
                }
                catch (Exception)
                {
                    return new string[0];
                }
            }
        }

        public override string MotherboardSerialNumber
        {
            get
            {
                try
                {
                    using var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard");
                    foreach (var obj in searcher.Get())
                    {
                        return obj["SerialNumber"]?.ToString() ?? string.Empty;
                    }
                    return string.Empty;
                }
                catch (Exception)
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
                    using var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS");
                    foreach (var obj in searcher.Get())
                    {
                        return obj["SerialNumber"]?.ToString() ?? string.Empty;
                    }
                    return string.Empty;
                }
                catch (Exception)
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
                    var properties = new Dictionary<string, object>();
                    
                    // Get display information
                    using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
                    foreach (var obj in searcher.Get())
                    {
                        properties["Width"] = obj["CurrentHorizontalResolution"] ?? 0;
                        properties["Height"] = obj["CurrentVerticalResolution"] ?? 0;
                        properties["RefreshRate"] = obj["CurrentRefreshRate"] ?? 0;
                        properties["BitsPerPixel"] = obj["CurrentBitsPerPixel"] ?? 0;
                        properties["AdapterName"] = obj["Name"]?.ToString() ?? string.Empty;
                        properties["AdapterRAM"] = obj["AdapterRAM"] ?? 0;
                        break; // Just get the first display adapter
                    }
                    
                    return properties;
                }
                catch (Exception)
                {
                    return new Dictionary<string, object>();
                }
            }
        }
    }
}
