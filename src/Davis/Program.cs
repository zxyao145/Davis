
using Blazored.SessionStorage;
using Davis.Core;
using Microsoft.Extensions.DependencyInjection;
using Photino.Blazor;

namespace Davis;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        var appBuilder = PhotinoBlazorAppBuilder
            .CreateDefault(args);

        appBuilder.Services
            .AddLogging();
        appBuilder.Services
            .AddBlazoredSessionStorage()
            .AddSingleton<PluginService>();

        // register root component and selector
        appBuilder.RootComponents.Add<App>("app");

        var app = appBuilder.Build();

        // customize window
        app.MainWindow
            .SetContextMenuEnabled(false)
            .SetTitle("Davis")
            .SetIconFile("favicon.ico")
            .SetWidth((int)(960 * 1.25)) // 320 * 3
            .SetHeight((int)(593 * 1.25));//240.6 * 3=578.4

        AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
        {
            app.MainWindow.OpenAlertWindow("Fatal exception", error.ExceptionObject.ToString());
        };

        app.Run();
    }

}

