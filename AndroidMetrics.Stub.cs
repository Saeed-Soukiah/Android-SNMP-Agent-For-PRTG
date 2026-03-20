#if !ANDROID
using System;

namespace PhoneSnmpAgent;

/// <summary>
/// This class provides a *stub* implementation of AndroidMetrics for non‑Android targets.
/// 
/// Why this file exists:
/// - Your MAUI project can compile for Windows, macOS, or Linux.
/// - The real AndroidMetrics class uses Android APIs that do NOT exist on desktop.
/// - Without this stub, the compiler would fail because the class is missing.
/// 
/// Purpose:
/// - Allow the rest of the SNMP agent code to compile on any platform.
/// - Provide placeholder values so the app can run without Android hardware.
/// 
/// Behavior:
/// - Every method returns a safe default value.
/// - These values are intentionally simple (0, -1, or "unknown").
/// - On Android, this file is excluded because of the #if !ANDROID directive.
/// </summary>
public static class AndroidMetrics
{
    /// <summary>Battery percentage is not available on non‑Android platforms.</summary>
    public static int GetBatteryLevel() => -1;

    /// <summary>Free RAM cannot be queried using Android APIs on desktop.</summary>
    public static long GetFreeRam() => -1;

    /// <summary>Free internal storage (Android Data partition) is not applicable.</summary>
    public static long GetFreeStorage() => -1;

    /// <summary>Cellular signal strength is Android‑only.</summary>
    public static int GetSignalStrength() => -1;

    /// <summary>Uptime is not tracked using Android's SystemClock on desktop.</summary>
    public static int GetUptimeSeconds() => 0;

    /// <summary>CPU usage via /proc/stat is Android/Linux‑specific.</summary>
    public static int GetCpuUsage() => -1;

    /// <summary>WiFi RSSI is not available without Android's WifiManager.</summary>
    public static int GetWifiRssi() => -1;

    /// <summary>Network type (WiFi/Mobile) is Android‑specific.</summary>
    public static int GetNetworkType() => 0;

    /// <summary>Battery temperature is not available on non‑Android systems.</summary>
    public static int GetBatteryTemperature() => -1;

    /// <summary>Charging state cannot be detected without Android's battery APIs.</summary>
    public static int GetChargingState() => 0;

    /// <summary>Mobile data usage is Android‑specific.</summary>
    public static int GetMobileDataUsageMB() => 0;

    /// <summary>WiFi link speed requires Android's WifiInfo.</summary>
    public static int GetWifiLinkSpeed() => -1;

    /// <summary>GPS accuracy cannot be retrieved without Android's LocationManager.</summary>
    public static int GetGpsAccuracy() => -1;

    /// <summary>Bluetooth status requires Android's BluetoothAdapter.</summary>
    public static int GetBluetoothStatus() => 0;

    /// <summary>App memory usage via ActivityManager is Android‑only.</summary>
    public static int GetAppMemoryUsageMB() => -1;

    /// <summary>Thermal state is not available on non‑Android systems.</summary>
    public static int GetThermalState() => 0;

    /// <summary>WiFi traffic counters are Android‑specific.</summary>
    public static int GetWifiTrafficMB() => 0;

    /// <summary>Mobile traffic counters are Android‑specific.</summary>
    public static int GetMobileTrafficMB() => 0;

    /// <summary>CPU temperature sensors are Android/Linux‑specific.</summary>
    public static int GetCpuTemperature() => -1;

    /// <summary>Screen state (interactive/on/off) is Android‑specific.</summary>
    public static int GetScreenState() => 0;

    /// <summary>Foreground app detection requires Android's UsageStatsManager.</summary>
    public static string GetForegroundApp() => "unknown";

    /// <summary>Battery health is not available on non‑Android platforms.</summary>
    public static int GetBatteryHealth() => -1;

    /// <summary>Cell tower ID requires Android's TelephonyManager.</summary>
    public static int GetCellTowerId() => -1;
}
#endif