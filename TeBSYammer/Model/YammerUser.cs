using System;
using Newtonsoft.Json;
namespace TeBSYammer.Model
{
    public class YammerUser
    {
        public YammerUser()
        {
        }


        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("full_name")]
        public string ImageUrl { get; set; }


    }
}
