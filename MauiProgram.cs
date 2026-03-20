using Microsoft.Extensions.Logging;

namespace PhoneSnmpAgent
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Android-only startup branding toast
            builder.ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                Microsoft.Maui.Handlers.PageHandler.Mapper.AppendToMapping("SplashBranding", (handler, view) =>
                {
                    Android.App.Application.SynchronizationContext.Post(_ =>
                    {
                        Android.Widget.Toast.MakeText(
                            Android.App.Application.Context,
                            "Developed by SSG © 2026",
                            Android.Widget.ToastLength.Long
                        ).Show();
                    }, null);
                });
#endif
            });

            return builder.Build();
        }
    }
}
