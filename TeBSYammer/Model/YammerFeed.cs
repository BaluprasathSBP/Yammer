using System;
using Newtonsoft.Json;

namespace TeBSYammer.Model
{
    [JsonObject]
    public class YammerFeed
    {
        public YammerFeed()
        {
        }

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("client_type")]
        public string ClientType { get; set; }

        [JsonProperty("body")]
        public Body MessageBody { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedOn {get;set; }

        public YammerUser User { get; set; }

        [JsonProperty("sender_id")]
        public string CreatedBy { get; set; }

    }

    
    public class Body
    {
        [JsonProperty("parsed")]
        public string ParsedText { get; set; }

        [JsonProperty("plain")]
        public string PlainText { get; set; }

        [JsonProperty("rich")]
        public string RichText { get; set; }
    }
}
