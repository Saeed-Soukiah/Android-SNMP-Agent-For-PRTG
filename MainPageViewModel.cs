#if ANDROID
using Android.Content;
#endif

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PhoneSnmpAgent;

public class MainPageViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public bool PermissionsGranted { get; set; } = false;

    private string _agentStatus = "Stopped";
    private Color _agentStatusColor = Colors.Red;

    public string AgentStatus
    {
        get => _agentStatus;
        set { _agentStatus = value; OnPropertyChanged(); }
    }

    public Color AgentStatusColor
    {
        get => _agentStatusColor;
        set { _agentStatusColor = value; OnPropertyChanged(); }
    }

    public ICommand StartAgentCommand => new Command(async () => await StartAgent());
    public ICommand StopAgentCommand => new Command(StopAgent);

    private async Task StartAgent()
    {
        if (!PermissionsGranted)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Permissions Required",
                "Please grant permissions before starting the SNMP agent.",
                "OK");
            return;
        }

#if ANDROID
        var intent = new Intent(Android.App.Application.Context, typeof(SnmpService));
        Android.App.Application.Context.StartForegroundService(intent);
#endif

        AgentStatus = "Running";
        AgentStatusColor = Colors.LimeGreen;
    }

    private void StopAgent()
    {
#if ANDROID
        var intent = new Intent(Android.App.Application.Context, typeof(SnmpService));
        Android.App.Application.Context.StopService(intent);
#endif

        AgentStatus = "Stopped";
        AgentStatusColor = Colors.Red;
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
