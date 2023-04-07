using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Davis.Core
{
    internal sealed class PluginContext : IPluginContext
    {
        public string Name { get; set; } = "";

        public string PluginRootDir => Path.Combine("Plugins", Name);

        public IServiceProvider ServiceProvider { get; }

        public PluginContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void SendNotification(string title, string message)
        {
            Program.App.MainWindow.SendNotification(title, message);
        }

        public void SendWebMessage(string message)
        {
            Program.App.MainWindow.SendWebMessage(message);
        }

        public async Task LoadJs(string file)
        {
            var url = Path.Combine(PluginRootDir, "wwwroot", file);

            var js = ServiceProvider.GetRequiredService<IJSRuntime>();
            await js.InvokeVoidAsync("Davis.loadJs", url);
        }

        public async Task UnLoadJs(string file)
        {
            var url = Path.Combine(PluginRootDir, "wwwroot", file);

            var js = ServiceProvider.GetRequiredService<IJSRuntime>();
            await js.InvokeVoidAsync("Davis.unloadJs", url);
        }

        public async Task LoadCss(string file)
        {
            var url = Path.Combine(PluginRootDir, "wwwroot", file);

            var js = ServiceProvider.GetRequiredService<IJSRuntime>();
            await js.InvokeVoidAsync("Davis.loadCss", url);
        }

        public async Task UnLoadCss(string file)
        {
            var url = Path.Combine(PluginRootDir, "wwwroot", file);
            var js = ServiceProvider.GetRequiredService<IJSRuntime>();
            await js.InvokeVoidAsync("Davis.unloadCss", url);
        }
    }
}
