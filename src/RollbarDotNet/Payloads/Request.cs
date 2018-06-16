namespace RollbarDotNet.Payloads
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class Request
    {
        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("cookies")]
        public Dictionary<string, string> Cookies { get; set; }

        [JsonProperty("GET")]
        public Dictionary<string, string> Get { get; set; }

        [JsonProperty("headers")]
        public Dictionary<string, string> Headers { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params")]
        public Dictionary<string, string> Parameters { get; set; }

        [JsonProperty("POST")]
        public Dictionary<string, string> Post { get; set; }

        [JsonProperty("query_string")]
        public string QueryString { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("user_ip")]
        public string UserIp { get; set; }
    }
}