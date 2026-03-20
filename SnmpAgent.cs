using SnmpSharpNet;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace PhoneSnmpAgent;

public class SnmpAgent
{
    private readonly int _port;
    private bool _running;
    private UdpClient? _udpClient;

    public SnmpAgent(int port = 16100)
    {
        _port = port;
    }

    public async Task StartAsync(CancellationToken token)
    {
        if (_running)
            return;

        _running = true;
        _udpClient = new UdpClient(_port);

        try
        {
            while (!token.IsCancellationRequested)
            {
                UdpReceiveResult result;

                try
                {
                    result = await _udpClient.ReceiveAsync(token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch
                {
                    continue;
                }

                try
                {
                    var request = new SnmpV2Packet();
                    request.decode(result.Buffer, result.Buffer.Length);

                    var response = HandleRequest(request);

                    byte[] responseBytes = response.encode();
                    await _udpClient.SendAsync(responseBytes, responseBytes.Length, result.RemoteEndPoint);
                }
                catch
                {
                    // Ignore malformed packets and continue
                }
            }
        }
        finally
        {
            _udpClient?.Close();
            _running = false;
        }
    }

    public void Stop()
    {
        _running = false;
        _udpClient?.Close();
    }

    private static int SafeInt(Func<int> getter)
    {
        try { return getter(); }
        catch { return -1; }
    }

    private static long SafeLong(Func<long> getter)
    {
        try { return getter(); }
        catch { return -1L; }
    }

    private static string SafeString(Func<string> getter)
    {
        try { return getter(); }
        catch { return "unknown"; }
    }

    private SnmpV2Packet HandleRequest(SnmpV2Packet request)
    {
        var response = request;
        response.Pdu.Type = PduType.Response;
        response.Pdu.ErrorStatus = 0;
        response.Pdu.ErrorIndex = 0;

        foreach (Vb vb in response.Pdu.VbList)
        {
            string oid = vb.Oid.ToString();

            if (oid == "1.3.6.1.4.1.55555.1.1.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetBatteryLevel()));

            else if (oid == "1.3.6.1.4.1.55555.1.2.0")
                vb.Value = new Integer32((int)SafeLong(() => AndroidMetrics.GetFreeRam()));

            else if (oid == "1.3.6.1.4.1.55555.1.3.0")
                vb.Value = new Integer32((int)SafeLong(() => AndroidMetrics.GetFreeStorage()));

            else if (oid == "1.3.6.1.4.1.55555.1.4.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetSignalStrength()));

            else if (oid == "1.3.6.1.4.1.55555.1.5.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetUptimeSeconds()));

            else if (oid == "1.3.6.1.4.1.55555.1.6.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetCpuUsage()));

            else if (oid == "1.3.6.1.4.1.55555.1.7.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetWifiRssi()));

            else if (oid == "1.3.6.1.4.1.55555.1.8.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetNetworkType()));

            else if (oid == "1.3.6.1.4.1.55555.1.9.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetBatteryTemperature()));

            else if (oid == "1.3.6.1.4.1.55555.1.10.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetChargingState()));

            else if (oid == "1.3.6.1.4.1.55555.1.11.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetMobileDataUsageMB()));

            else if (oid == "1.3.6.1.4.1.55555.1.12.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetWifiLinkSpeed()));

            else if (oid == "1.3.6.1.4.1.55555.1.13.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetGpsAccuracy()));

            else if (oid == "1.3.6.1.4.1.55555.1.14.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetBluetoothStatus()));

            else if (oid == "1.3.6.1.4.1.55555.1.15.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetAppMemoryUsageMB()));

            else if (oid == "1.3.6.1.4.1.55555.1.16.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetThermalState()));

            else if (oid == "1.3.6.1.4.1.55555.1.17.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetWifiTrafficMB()));

            else if (oid == "1.3.6.1.4.1.55555.1.18.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetMobileTrafficMB()));

            else if (oid == "1.3.6.1.4.1.55555.1.19.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetCpuTemperature()));

            else if (oid == "1.3.6.1.4.1.55555.1.20.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetScreenState()));

            else if (oid == "1.3.6.1.4.1.55555.1.21.0")
                vb.Value = new OctetString(SafeString(() => AndroidMetrics.GetForegroundApp()));

            else if (oid == "1.3.6.1.4.1.55555.1.22.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetBatteryHealth()));

            else if (oid == "1.3.6.1.4.1.55555.1.23.0")
                vb.Value = new Integer32(SafeInt(() => AndroidMetrics.GetCellTowerId()));

            else
                vb.Value = new NoSuchObject();
        }

        return response;
    }
}
