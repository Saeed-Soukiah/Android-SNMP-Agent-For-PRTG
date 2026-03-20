# 📱 Android SNMP Agent for PRTG

![Build](https://img.shields.io/badge/build-passing-brightgreen)
![License](https://img.shields.io/badge/license-MIT-blue)
![Framework](https://img.shields.io/badge/framework-.NET%20MAUI-purple)
![Monitoring](https://img.shields.io/badge/monitoring-PRTG-yellow)

An **Android SNMP Agent** built with .NET MAUI, designed to show Android device metrics (battery, CPU, RAM, storage, network, etc.) to **PRTG Network Monitor**.  
Partially Tested successfully on **Sony**, **Samsung**, and on **Xiaomi** (requires MIUI tweaks).

---

## 📱 Android Compatibility

The application supports a wide range of Android versions due to MAUI’s default configuration.

### ✔ **Supported Android Versions**
The app runs on:

- **Android 5.0 (API 21)**  
- Up to **Android 14 (API 34)**  
- And newer versions automatically (`AndroidUseLatestPlatformSdk=true`)

### ⭐ **Recommended Minimum Version**
For best performance, stability, and full metric support:

- **Android 8.0+ (API 26)** — recommended baseline  
- **Android 10+ (API 29)** — ideal for background services  
- **Android 12+ (API 31)** — recommended for modern devices and full compatibility

Older Android versions may restrict:

- WiFi RSSI  
- CPU temperature  
- Thermal state  
- Background service reliability  

---

## ✔ Tested with PRTG Network Monitor

This application has been fully tested with **PRTG Network Monitor**.  
The following were verified:

- SNMP v2c communication on port **16100**
- All OIDs responding correctly using `snmpwalk`
- Successful import of the custom MIB file
- Automatic sensor creation using **SNMP Library Sensors**
- Stable long‑term monitoring on Sony and Samsung devices

---

## 📥 Importing the MIB File into PRTG (Using Paessler MIB Importer v3)

### **1. Download Paessler MIB Importer v3**

Download from Paessler’s official website.

---

### **2. Open Your MIB File in the Importer**

1. Launch **Paessler MIB Importer v3**  
2. Click **File → Open**  
3. Select your `.mib` file  
4. The importer will parse and display all OIDs

Warnings about missing dependencies are normal.

---

### **3. Convert the MIB to an OIDLIB File**

1. Click **File → Save as OIDLIB**  
2. Save as:

```
PhoneSnmpAgent.oidlib
```

---

### **4. Copy the OIDLIB File to PRTG**

Place the `.oidlib` file here:

```
C:\Program Files (x86)\PRTG Network Monitor\snmplibs\
```

Create the folder if needed.

---

### **5. Restart the PRTG Probe Service**

Restart from:

- Windows Services  
- Or PRTG Administration Tool

---

### **6. Add an SNMP Library Sensor in PRTG**

1. Open PRTG  
2. Select your Android device  
3. Click **Add Sensor**  
4. Search for **SNMP Library**  
5. Choose:

```
PhoneSnmpAgent.oidlib
```

6. Select metrics  
7. Finish the wizard

---

### **7. Verify Sensor Values**

PRTG will begin polling and show live values for:

- Battery  
- CPU  
- RAM  
- Storage  
- Network  
- GPS  
- Device state  

---

## 📊 Supported Metrics

| OID | Metric | Type | Notes |
|-----|--------|------|-------|
| **1.3.6.1.4.1.55555.1.1.0** | Battery Level (%) | Integer | Universal |
| **1.3.6.1.4.1.55555.1.2.0** | Free RAM (MB) | Integer | Universal |
| **1.3.6.1.4.1.55555.1.3.0** | Free Storage (MB) | Integer | Universal |
| **1.3.6.1.4.1.55555.1.4.0** | Signal Strength (dBm) | Integer | Samsung preferred |
| **1.3.6.1.4.1.55555.1.5.0** | Uptime (seconds) | Integer | Universal |
| **1.3.6.1.4.1.55555.1.6.0** | CPU Usage (%) | Integer | Universal |
| **1.3.6.1.4.1.55555.1.7.0** | WiFi RSSI (dBm) | Integer | Samsung preferred |
| **1.3.6.1.4.1.55555.1.8.0** | Network Type | Integer | 0=Unknown, 1=WiFi, 2=Mobile |
| **1.3.6.1.4.1.55555.1.9.0** | Battery Temperature (°C) | Integer | Universal |
| **1.3.6.1.4.1.55555.1.10.0** | Charging State | Integer | 0=No, 1=Yes |
| **1.3.6.1.4.1.55555.1.11.0** | Mobile Data Usage (MB) | Integer | Universal |
| **1.3.6.1.4.1.55555.1.12.0** | WiFi Link Speed (Mbps) | Integer | Samsung only |
| **1.3.6.1.4.1.55555.1.13.0** | GPS Accuracy (m) | Integer | Requires GPS |
| **1.3.6.1.4.1.55555.1.14.0** | Bluetooth Status | Integer | 0=Off, 1=On |
| **1.3.6.1.4.1.55555.1.15.0** | App Memory Usage (MB) | Integer | Universal |
| **1.3.6.1.4.1.55555.1.16.0** | Thermal State | Integer | Samsung only |
| **1.3.6.1.4.1.55555.1.17.0** | WiFi Traffic (MB) | Integer | Universal |
| **1.3.6.1.4.1.55555.1.18.0** | Mobile Traffic (MB) | Integer | Universal |
| **1.3.6.1.4.1.55555.1.19.0** | CPU Temperature (°C) | Integer | Samsung only |
| **1.3.6.1.4.1.55555.1.20.0** | Screen State | Integer | 0=Off, 1=On |
| **1.3.6.1.4.1.55555.1.21.0** | Foreground App | String | Package name |
| **1.3.6.1.4.1.55555.1.22.0** | Battery Health | Integer | Universal |
| **1.3.6.1.4.1.55555.1.23.0** | Cell Tower ID | Integer | Samsung only |

---

## 🛠 Installation

### On Android Device
1. Install the APK  
4. Open app → tap **Start SNMP Agent**  
5. Confirm persistent notification: *“SNMP Agent Running”*

### On Xiaomi Devices (MIUI)
- Disable battery optimization  
- Enable autostart  
- Lock app in recent apps  
- Allow background data + unrestricted data  
- Enable “Allow background pop‑ups”  

---

## 🔧 PRTG Setup

1. Add device in PRTG with your phone’s IP  
2. Set SNMP version = **v2c**  
3. Set community = **public**  
4. Set port = **16100**  
5. Add **SNMP Custom Sensor** or **SNMP Library Sensor**  
6. Use OIDs from the table above

---

## 📈 Example PRTG Dashboard Layout

| Section          | Sensors |
|------------------|---------|
| **Battery & Power** | Battery Level, Temp, Health, Charging |
| **Performance**     | CPU Usage, CPU Temp, RAM, App Memory |
| **Storage**         | Free Storage |
| **Network**         | WiFi RSSI, Link Speed, Traffic, Mobile Traffic, Signal Strength, Cell Tower ID |
| **Environment**     | GPS Accuracy, Thermal State |
| **Device State**    | Screen State, Uptime |

---

## 🖼 Screenshots

### 📱 App UI  
<img width="448" height="947" alt="Screenshot 2026-03-20 235607" src="https://github.com/user-attachments/assets/8848465f-62c0-4c57-9885-cb75706ad4ca" />

<img width="443" height="944" alt="Screenshot 2026-03-20 235619" src="https://github.com/user-attachments/assets/a9dfe6a4-e7ce-476c-9bee-d7782db576ac" />

---

## 📌 Notes

- Sony: baseline metrics only  
- Samsung: extended metrics (thermal, WiFi, signal strength)  
- Xiaomi: requires MIUI tweaks  
- Works with **PRTG SNMP Custom Sensors** and **SNMP Library Sensors**  

---

## 🤝 Contributing

Contributions are welcome! If you’d like to improve the project, add new metrics, fix bugs, or enhance compatibility, All contributions — big or small — are appreciated.

---

## 📜 License

MIT License — free to use, modify, and distribute.

---
