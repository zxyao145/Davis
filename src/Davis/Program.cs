
using Blazored.SessionStorage;
using Davis.Core;
using Microsoft.Extensions.DependencyInjection;
using OneOf.Types;
using Photino.Blazor;
using PhotinoNET;
using System.Text.Json;

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
            .AddAntDesign()
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
            .SetUseOsDefaultSize(false)
            .SetWidth((int)(960 * 1.25)) // 320 * 3
            .SetHeight((int)(593 * 1.25)) //240.6 * 3=578.4
            .SetBrowserControlInitParameters(GetBrowserControlInitParameters())
            ;

        app.MainWindow.Centered = true;

        AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
        {
            app.MainWindow.ShowMessage("Fatal exception", error.ExceptionObject.ToString());
        };
        app.MainWindow.RegisterCustomSchemeHandler("davis", (object sender, string scheme, string url, out string contentType) =>
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
            var urlPath = url.Substring("davis://".Length);// davis://MessagePlugin/wwwroot/index.js
            var filePath = Path.Combine(AppContext.BaseDirectory, urlPath);

            if (!File.Exists(filePath))
            {
                app.MainWindow.ShowMessage("not fount assets", url);
                return Stream.Null;
            }
            try
            {
                var sr = new StreamReader(filePath);
                return sr.BaseStream;
            }
            catch (Exception e)
            {
                app.MainWindow.ShowMessage("Fatal exception", e.Message);
                return Stream.Null;
            }
        });

        app.Run();
    }

    private static string GetBrowserControlInitParameters()
    {
        string browserControlInitParams = "--flag --kiosk=false";

        return browserControlInitParams;
    }
}

