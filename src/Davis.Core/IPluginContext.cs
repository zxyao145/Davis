namespace Davis.Core
{
    public interface IPluginContext
    {
        string Name { get; }    

        string PluginRootDir { get; }

        IServiceProvider ServiceProvider { get; }

        void SendNotification(string title, string message);

        void SendWebMessage(string message);

        Task LoadJs(string file);

        Task UnLoadJs(string file);

        Task LoadCss(string file);

        Task UnLoadCss(string file);
    }
}
