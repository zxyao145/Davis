using System.Reflection;

namespace Davis.Core
{
    internal class PluginService
    {
        public Dictionary<string, Type> pluginCache = new Dictionary<string, Type>();

        public (Type?, string msg) GetComponent(string pluginFileName)
        {
            if (pluginCache.ContainsKey(pluginFileName))
            {
                return (pluginCache[pluginFileName], "");
            }
            var cur = AppContext.BaseDirectory;
            cur = Path.Combine(cur, $"Plugins/{pluginFileName}.dll");
            if (System.IO.File.Exists(cur))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(cur);
                    Type? componentType = assembly.GetTypes()
                        .FirstOrDefault(t => 
                            t.GetInterfaces().Contains(typeof(IDavisPlugin)
                        ));
                    if(componentType != null)
                    {
                        pluginCache.Add(pluginFileName, componentType);
                        return (componentType, "");
                    }
                    return (null, "");
                }
                catch (Exception e)
                {
                    return (null, e.Message); ;
                }
               
            }
            return (null, "plugin not found!");
        }
    }
}
