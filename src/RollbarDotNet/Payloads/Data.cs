namespace RollbarDotNet.Payloads
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    [JsonObject(MemberSerialization.OptIn)]
    public class Data
    {
        public Data()
        {
            this.Body = new Body();
        }

        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("body")]
        public Body Body { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("code_version")]
        public string CodeVersion { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("framework")]
        public string Framework { get; set; }

        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("request")]
        public Request Request { get; set; }

        [JsonProperty("person")]
        public Person Person { get; set; }

        [JsonProperty("server")]
        public Server Server { get; set; }

        [JsonProperty("client")]
        public Client Client { get; set; }

        [JsonProperty("custom")]
        public Dictionary<string, object> Custom { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("notifier")]
        public Notifier Notifier { get; set; }
    }
}
