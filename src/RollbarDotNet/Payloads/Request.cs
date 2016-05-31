namespace RollbarDotNet.Payloads
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    [JsonObject(MemberSerialization.OptIn)]
    public class Request
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("headers")]
        public Dictionary<string, string> Headers { get; set; }

        [JsonProperty("params")]
        public Dictionary<string, string> Parameters { get; set; }

        [JsonProperty("GET")]
        public Dictionary<string, string> Get { get; set; }

        [JsonProperty("query_string")]
        public string QueryString { get; set; }

        [JsonProperty("POST")]
        public Dictionary<string, string> Post { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("user_ip")]
        public string UserIp { get; set; }
    }
}
