using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Davis.Models
{
    internal class PluginInfo
    {
        public string Name { get; set; } = "";
        public string FileName { get; set; } = "";
        public string Version { get; set; } = "0.0.0";
        public string Author { get; set; } = "nameless";
        public string Description { get; set; } = "";
        public List<string> Tags { get; set; }  = new List<string>();

        public string GetFileNameUrl()
        {
            return UrlEncoder.Default.Encode(FileName);
        }
    }
}
