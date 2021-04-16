using Configs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlacklistMonitor.Configurations
{
    class AppConfigs : ConfigsTools
    {
        [JsonProperty("activated", Required = Required.AllowNull)]
        public bool Activated { get; set; }
    }
}
