using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CsRegexPlugin
{
    public partial class Plugin
    {
        private string _pattern = "";

        private void OnRegexStrChange(ChangeEventArgs e)
        {
            _pattern = e.Value?.ToString() ?? "";
        }

        private string _txt = "";

        private void OnTxtChange(ChangeEventArgs e)
        {
            _txt = e.Value?.ToString() ?? "";
        }

        private MatchCollection? _matchCollection = null;

        private Task OnOk(MouseEventArgs e)
        {
            var regex = new Regex(_pattern);
            _matchCollection = regex.Matches(_txt);
            return Task.CompletedTask;
        }
    }
}
