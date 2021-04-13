using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Core.Shared
{
    public class BrowserInfo
    {
        [JsonProperty("ips")]
        public List<string> Ips { get; set; }

        [JsonProperty("browserId")]
        public Guid? BrowserId { get; set; }
        [JsonProperty("os")]
        public OsInfo Os { get; set; }


        public bool Validate()
        {
            if ((Ips == null || Ips.Count == 0) && !BrowserId.HasValue)
            {
                return false;
            }
            if (Os == null)
            {
                return false;
            }
            return true;

        }
    }

    public class OsInfo
    {
        [JsonProperty("browser")]
        public string Browser { get; set; }

        [JsonProperty("browserMajorVersion")]
        public int? BrowserMajorVersion { get; set; }

        [JsonProperty("browserVersion")]
        public string BrowserVersion { get; set; }

        [JsonProperty("cookies")]
        public bool? Cookies { get; set; }

        [JsonProperty("flashVersion")]
        public string FlashVersion { get; set; }

        [JsonProperty("mobile")]
        public bool? Mobile { get; set; }

        [JsonProperty("os")]
        public string Os { get; set; }

        [JsonProperty("osVersion")]
        public string OsVersion { get; set; }

        [JsonProperty("screen")]
        public string Screen { get; set; }
    }
}
