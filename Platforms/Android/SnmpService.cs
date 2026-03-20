#if ANDROID
using Android.App;
using Android.Content;
using Android.OS;
using System.Threading;

namespace PhoneSnmpAgent;

[Service(
    Exported = true,
    ForegroundServiceType = Android.Content.PM.ForegroundService.TypeDataSync)]
public class SnmpService : Service
{
    private CancellationTokenSource? _cts;
    private SnmpAgent? _agent;

    public override IBinder? OnBind(Intent? intent) => null;

    public override void OnCreate()
    {
        base.OnCreate();

        _cts = new CancellationTokenSource();

        try
        {
            _agent = new SnmpAgent(16100);
            StartForegroundNotification();
            _ = _agent.StartAsync(_cts.Token);
        }
        catch (Exception ex)
        {
            Android.Util.Log.Error("SNMP", ex.ToString());
        }
    }

    public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
    {
        return StartCommandResult.Sticky;
    }

    public override void OnDestroy()
    {
        try
        {
            _cts?.Cancel();
            _agent?.Stop();
        }
        finally
        {
            base.OnDestroy();
        }
    }

    private void StartForegroundNotification()
    {
        const string channelId = "snmp_agent_channel";
        const string channelName = "SNMP Agent";

        var nm = (NotificationManager)GetSystemService(NotificationService)!;

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channel = new NotificationChannel(
                channelId,
                channelName,
                NotificationImportance.Low);

            nm.CreateNotificationChannel(channel);
        }

        var notification = new Notification.Builder(this, channelId)
        .SetContentTitle("SNMP Agent Running")
        .SetContentText("PRTG can now monitor this phone via SNMP.")
        .SetSmallIcon(Android.Resource.Drawable.IcDialogInfo)
        .SetOngoing(true)
        .Build();


        StartForeground(1, notification);
    }
}
#endif
