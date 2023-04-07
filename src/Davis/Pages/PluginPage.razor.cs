using Davis.Core;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Davis.Pages
{
    public partial class PluginPage
    {
        [Inject]
        internal PluginService PluginService { get; set; } = default!;

        [Inject]
        public IPluginContext Context { get; set; } = default!;


        [Parameter, EditorRequired]
        public string PluginName { get; set; } = "";

        private string _msg = "loading";
        private Type? _dynamicType = null;
        private DynamicComponent? _dynamicConponentRef;

        protected override void OnParametersSet()
        {
            var decodePluginName = HttpUtility.UrlDecode(PluginName);
            ((PluginContext)Context).Name = decodePluginName.Split('/', '\\')[0];

            var (componentType, msg) = PluginService.GetComponent(decodePluginName);
            if (componentType == null)
            {
                _dynamicType = null;
                _msg = msg;
            }
            else
            {
                _dynamicType = componentType;
            }
            StateHasChanged();
        }

    }
}
