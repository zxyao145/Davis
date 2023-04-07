
using Blazored.SessionStorage;
using Davis.Models;
using Jil;
using Microsoft.AspNetCore.Components;
using System;
using System.Net;

namespace Davis.Pages
{
    public partial class Index
    {
        [Inject]
        ISessionStorageService SessionStorage { get; set; } = default!;


        private readonly List<PluginInfo> _plugins = new List<PluginInfo>();

        private List<PluginInfo> _renderPlugins = new List<PluginInfo>();

        private string _searchTxt = "";
        private ElementReference _searchInput;

        protected override async Task OnInitializedAsync()
        {
            await LoadPluginMetaAsync();
            await base.OnInitializedAsync();
        }

        private async Task LoadPluginMetaAsync()
        {
            var pluginRoots = Path.Combine(AppContext.BaseDirectory, "Plugins");
            var subDirs = Directory.GetDirectories(pluginRoots);
            var pluginInfos = new List<PluginInfo>();
            foreach (var subDir in subDirs)
            {
                var subDirName = Path.GetFileName(subDir);
                var dllName = subDirName;

                var dll = Path.Combine(subDir, dllName + ".dll");
                if (!File.Exists(dll))
                {
                    continue;
                }

                var metaPath = Path.Combine(subDir, "_meta.json");
                PluginInfo pluginInfo;
                if (File.Exists(metaPath))
                {
                    var allTxt = await File.ReadAllTextAsync(metaPath);
                    pluginInfo = JSON.Deserialize<PluginInfo>
                        (allTxt, new Options(serializationNameFormat: SerializationNameFormat.CamelCase));

                }
                else
                {
                    pluginInfo = new PluginInfo();
                    pluginInfo.Description = "插件中不存在 _meta.json 文件";
                }

                pluginInfo.FileName = subDirName + "/" + dllName;

                if (string.IsNullOrWhiteSpace(pluginInfo.Name))
                {
                    pluginInfo.Name = subDirName;
                }
                pluginInfos.Add(pluginInfo);
            }

            var files = Directory.GetFiles(pluginRoots, "*.dll");
            foreach (var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file);
                var pluginInfo = new PluginInfo();
                pluginInfo.Name = name;
                pluginInfo.FileName = name;
                pluginInfo.Description = "简单插件";
                pluginInfos.Add(pluginInfo);
            }
            _plugins.Clear();
            _plugins.AddRange(pluginInfos.OrderBy(x => x.Name));
            _searchTxt = await SessionStorage.GetItemAsStringAsync(nameof(_searchTxt));

            await SetSearchTxt(_searchTxt);
        }


        private async Task ClearSearchTxt()
        {
            await _searchInput.FocusAsync();
            await SetSearchTxt("");
        }

        private async Task OnSearchTxtChange(ChangeEventArgs e)
        {
            await SetSearchTxt(e.Value?.ToString() ?? "");
        }

        private async Task SetSearchTxt(string txt)
        {
            _searchTxt = txt ?? "";
            await SessionStorage.SetItemAsStringAsync(nameof(_searchTxt), _searchTxt);

            if (string.IsNullOrWhiteSpace(_searchTxt))
            {
                _renderPlugins = _plugins;
            }
            else
            {
                _renderPlugins = 
                    _plugins
                    .Where(x => Match(x, _searchTxt))
                    .ToList();
            }
            StateHasChanged();
        }

        private static bool Match(PluginInfo pluginInfo, string searchTxt)
        {
            if (string.IsNullOrWhiteSpace(searchTxt))
            {
                return true;
            }

            bool IngoreCaseContain(string originStr, string value)
            {
                return originStr.Contains(searchTxt, StringComparison.CurrentCultureIgnoreCase);
            }

            if (IngoreCaseContain(pluginInfo.Name, searchTxt))
            {
                return true;
            }
            foreach (var tag in pluginInfo.Tags)
            {
                if (IngoreCaseContain(tag, searchTxt))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
