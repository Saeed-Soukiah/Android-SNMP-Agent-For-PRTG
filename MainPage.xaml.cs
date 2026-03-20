namespace PhoneSnmpAgent;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainPageViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var loc = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        var phone = await Permissions.RequestAsync<Permissions.Phone>();

        if (loc != PermissionStatus.Granted || phone != PermissionStatus.Granted)
        {
            await DisplayAlert("Permissions Required",
                "The SNMP agent cannot run without Location and Phone permissions.",
                "OK");
            return;
        }

        if (BindingContext is MainPageViewModel vm)
            vm.PermissionsGranted = true;
    }
}
