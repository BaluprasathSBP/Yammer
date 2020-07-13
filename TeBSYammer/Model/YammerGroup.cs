using System;
using Newtonsoft.Json;

namespace TeBSYammer.Model
{
    public class YammerGroup
    {
        [JsonProperty("id")]
        public string GroupId { get; set; }

        [JsonProperty("full_name")]
        public string GroupName { get; set; }

    }
}
