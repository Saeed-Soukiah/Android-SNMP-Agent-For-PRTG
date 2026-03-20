#if ANDROID
using Android.Content;
using Android.App;
using Android.Bluetooth;
using Android.Locations;
using Android.Net;
using Android.Net.Wifi;
using Android.OS;
using Android.Telephony;
using System;
using System.IO;
using System.Linq;

namespace PhoneSnmpAgent;

public static class AndroidMetrics
{
    private static Android.Content.Context Ctx => Android.App.Application.Context;

    public static int GetBatteryLevel()
    {
        try
        {
            var filter = new IntentFilter(Intent.ActionBatteryChanged);
            var battery = Ctx.RegisterReceiver(null, filter);
            return battery?.GetIntExtra(BatteryManager.ExtraLevel, -1) ?? -1;
        }
        catch { return -1; }
    }

    public static long GetFreeRam()
    {
        try
        {
            var am = (ActivityManager)Ctx.GetSystemService(Context.ActivityService);
            ActivityManager.MemoryInfo mem = new();
            am.GetMemoryInfo(mem);
            return mem.AvailMem / 1024 / 1024;
        }
        catch { return -1; }
    }

    public static long GetFreeStorage()
    {
        try
        {
            var stat = new StatFs(Android.OS.Environment.DataDirectory.Path);
            return stat.AvailableBytes / 1024 / 1024;
        }
        catch { return -1; }
    }

    public static int GetSignalStrength()
    {
        try
        {
            var tm = (TelephonyManager)Ctx.GetSystemService(Context.TelephonyService);
            var info = tm?.SignalStrength;
            if (info == null) return -1;

            var lte = info.CellSignalStrengths.OfType<CellSignalStrengthLte>().FirstOrDefault();
            if (lte != null) return lte.Dbm;

            var nr = info.CellSignalStrengths.OfType<CellSignalStrengthNr>().FirstOrDefault();
            if (nr != null) return nr.Dbm;

            var gsm = info.CellSignalStrengths.OfType<CellSignalStrengthGsm>().FirstOrDefault();
            if (gsm != null) return gsm.Dbm;

            var wcdma = info.CellSignalStrengths.OfType<CellSignalStrengthWcdma>().FirstOrDefault();
            if (wcdma != null) return wcdma.Dbm;

            return -1;
        }
        catch { return -1; }
    }

    public static int GetUptimeSeconds()
    {
        try { return (int)(SystemClock.ElapsedRealtime() / 1000); }
        catch { return -1; }
    }

    public static int GetCpuUsage()
    {
        try
        {
            using var reader = new StreamReader("/proc/stat");
            string line = reader.ReadLine() ?? "";
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 5) return -1;

            long user = long.Parse(parts[1]);
            long nice = long.Parse(parts[2]);
            long system = long.Parse(parts[3]);
            long idle = long.Parse(parts[4]);

            long total = user + nice + system + idle;
            long active = total - idle;

            if (total == 0) return -1;
            return (int)(active * 100 / total);
        }
        catch { return -1; }
    }

    public static int GetWifiRssi()
    {
        try
        {
            var wifi = (WifiManager)Ctx.GetSystemService(Context.WifiService);
            return wifi?.ConnectionInfo?.Rssi ?? -1;
        }
        catch { return -1; }
    }

    public static int GetNetworkType()
    {
        try
        {
            var cm = (ConnectivityManager)Ctx.GetSystemService(Context.ConnectivityService);
            var active = cm?.ActiveNetworkInfo;

            if (active == null || !active.IsConnected)
                return 0;

            return active.Type switch
            {
                ConnectivityType.Wifi => 1,
                ConnectivityType.Mobile => 2,
                _ => 0
            };
        }
        catch { return 0; }
    }

    public static int GetBatteryTemperature()
    {
        try
        {
            var filter = new IntentFilter(Intent.ActionBatteryChanged);
            var battery = Ctx.RegisterReceiver(null, filter);
            int temp = battery?.GetIntExtra(BatteryManager.ExtraTemperature, -1) ?? -1;
            return temp == -1 ? -1 : temp / 10;
        }
        catch { return -1; }
    }

    public static int GetChargingState()
    {
        try
        {
            var filter = new IntentFilter(Intent.ActionBatteryChanged);
            var battery = Ctx.RegisterReceiver(null, filter);
            int status = battery?.GetIntExtra(BatteryManager.ExtraStatus, -1) ?? -1;

            return status switch
            {
                (int)BatteryStatus.Charging => 1,
                (int)BatteryStatus.Full => 2,
                _ => 0
            };
        }
        catch { return 0; }
    }

    public static int GetMobileDataUsageMB()
    {
        try
        {
            long rx = Android.Net.TrafficStats.MobileRxBytes;
            long tx = Android.Net.TrafficStats.MobileTxBytes;
            return (int)((rx + tx) / 1024 / 1024);
        }
        catch { return -1; }
    }

    public static int GetWifiLinkSpeed()
    {
        try
        {
            var wifi = (WifiManager)Ctx.GetSystemService(Context.WifiService);
            return wifi?.ConnectionInfo?.LinkSpeed ?? -1;
        }
        catch { return -1; }
    }

    public static int GetGpsAccuracy()
    {
        try
        {
            var lm = (LocationManager)Ctx.GetSystemService(Context.LocationService);
            var providers = lm?.GetProviders(true);
            if (providers == null) return -1;

            foreach (var p in providers)
            {
                var loc = lm.GetLastKnownLocation(p);
                if (loc != null)
                    return (int)loc.Accuracy;
            }
            return -1;
        }
        catch { return -1; }
    }

    public static int GetBluetoothStatus()
    {
        try
        {
            var adapter = BluetoothAdapter.DefaultAdapter;
            return adapter?.IsEnabled == true ? 1 : 0;
        }
        catch { return 0; }
    }

    public static int GetAppMemoryUsageMB()
    {
        try
        {
            var am = (ActivityManager)Ctx.GetSystemService(Context.ActivityService);
            int pid = Android.OS.Process.MyPid();
            var info = am.GetProcessMemoryInfo(new int[] { pid });
            return info[0].TotalPss / 1024;
        }
        catch { return -1; }
    }

    public static int GetThermalState() => 0;

    public static int GetWifiTrafficMB()
    {
        try
        {
            long rx = Android.Net.TrafficStats.TotalRxBytes - Android.Net.TrafficStats.MobileRxBytes;
            long tx = Android.Net.TrafficStats.TotalTxBytes - Android.Net.TrafficStats.MobileTxBytes;
            return (int)((rx + tx) / 1024 / 1024);
        }
        catch { return -1; }
    }

    public static int GetMobileTrafficMB()
    {
        try
        {
            long rx = Android.Net.TrafficStats.MobileRxBytes;
            long tx = Android.Net.TrafficStats.MobileTxBytes;
            return (int)((rx + tx) / 1024 / 1024);
        }
        catch { return -1; }
    }

    public static int GetCpuTemperature()
    {
        try
        {
            string[] paths =
            {
                "/sys/class/thermal/thermal_zone0/temp",
                "/sys/class/hwmon/hwmon0/temp1_input"
            };

            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    var text = File.ReadAllText(path).Trim();
                    if (int.TryParse(text, out int milli))
                        return milli / 1000;
                }
            }
        }
        catch { }

        return -1;
    }

    public static int GetScreenState()
    {
        try
        {
            var pm = (PowerManager)Ctx.GetSystemService(Context.PowerService);
            return pm?.IsInteractive == true ? 1 : 0;
        }
        catch { return 0; }
    }

    public static string GetForegroundApp()
    {
        try
        {
            var usm = (Android.App.Usage.UsageStatsManager)Ctx.GetSystemService("usagestats");
            long end = Java.Lang.JavaSystem.CurrentTimeMillis();
            long begin = end - 60_000;

            var stats = usm?.QueryUsageStats(Android.App.Usage.UsageStatsInterval.Daily, begin, end);
            if (stats == null || !stats.Any())
                return "unknown";

            var last = stats.OrderBy(s => s.LastTimeUsed).LastOrDefault();
            return last?.PackageName ?? "unknown";
        }
        catch { return "unknown"; }
    }

    public static int GetBatteryHealth()
    {
        try
        {
            var filter = new IntentFilter(Intent.ActionBatteryChanged);
            var battery = Ctx.RegisterReceiver(null, filter);
            return battery?.GetIntExtra(BatteryManager.ExtraHealth, -1) ?? -1;
        }
        catch { return -1; }
    }

    public static int GetCellTowerId()
    {
        try
        {
            var tm = (TelephonyManager)Ctx.GetSystemService(Context.TelephonyService);
            var list = tm?.AllCellInfo;
            var reg = list?.FirstOrDefault(c => c.IsRegistered);

            if (reg is CellInfoLte lte)
                return lte.CellIdentity.Ci;

            if (reg is CellInfoGsm gsm)
                return gsm.CellIdentity.Cid;

            if (reg is CellInfoWcdma wcdma)
                return wcdma.CellIdentity.Cid;

            return -1;
        }
        catch { return -1; }
    }
}
#endif
