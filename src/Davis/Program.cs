
using Blazored.SessionStorage;
using Davis.Core;
using Microsoft.Extensions.DependencyInjection;
using Photino.Blazor;

namespace Davis;

class Program
{
    public static PhotinoBlazorApp App { get; private set; } = default!;

    [STAThread]
    static void Main(string[] args)
    {
        var appBuilder = PhotinoBlazorAppBuilder
            .CreateDefault(args);

        appBuilder.Services
            .AddLogging();
        appBuilder.Services
            .AddBlazoredSessionStorage()
            .AddSingleton<PluginService>()
            .AddScoped<IPluginContext, PluginContext>();

        // register root component and selector
        appBuilder.RootComponents.Add<App>("app");

        var app = appBuilder.Build();
        App = app;

        // customize window
        app.MainWindow
            .SetContextMenuEnabled(false)
            .SetGrantBrowserPermissions(true)
            .SetChromeless(false)
            .SetTitle("Davis")
            .SetIconFile("logo.ico")
            .SetWidth((int)(960 * 1.25)) // 320 * 3
            .SetHeight((int)(593 * 1.25));//240.6 * 3=578.4
        app.MainWindow.Centered = true;

        AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
        {
            app.MainWindow.OpenAlertWindow("Fatal exception", error.ExceptionObject.ToString());
        };
        app.MainWindow.RegisterCustomSchemeHandler("app", (object sender, string scheme, string url, out string contentType) =>
        {
            if (url.EndsWith(".js"))
            {
                contentType = "text/javascript";
            }
            else if (url.EndsWith(".css"))
            {
                contentType = "text/css";
            }
            else
            {
                contentType = "";
            }
            var urlPath = url.Substring("app://".Length);// app://MessagePlugin/wwwroot/index.js
            var filePath = Path.Combine(AppContext.BaseDirectory, urlPath);
            var sr = new StreamReader(filePath);
            return sr.BaseStream;
        });

        app.Run();
    }
}

