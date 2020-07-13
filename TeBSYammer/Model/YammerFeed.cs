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
        public DateTime CreatedOn { get; set; }

        public YammerUser User { get; set; }

        [JsonProperty("sender_id")]
        public string CreatedBy { get; set; }

        [JsonProperty("group_id")]
        public string GroupId { get; set; }

        public YammerGroup Group { get; set; }


    }


    public class Body
    {
        [JsonProperty("parsed")]
        public string ParsedText { get; set; }

        [JsonProperty("plain")]
        public string PlainText { get; set; }

        [JsonProperty("rich")]
        public string RichText
        {
            get
            {
                return _richText;
            }
            set
            {
                if (value != null)
                {
                    _richText = $"<div style =\"font-size: 14px;\">{value}</div>";
                }
                else
                {
                    _richText = value;
                }
            }
        }


        private string _richText;
    }
}
