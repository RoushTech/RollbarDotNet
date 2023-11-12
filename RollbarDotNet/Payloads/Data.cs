﻿namespace RollbarDotNet.Payloads
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class Data
    {
        [JsonProperty("body")]
        public Body Body { get; set; }

        [JsonProperty("client")]
        public Client Client { get; set; }

        [JsonProperty("code_version")]
        public string CodeVersion { get; set; }

        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("custom")]
        public Dictionary<string, object> Custom { get; set; }

        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }

        [JsonProperty("framework")]
        public string Framework { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("notifier")]
        public Notifier Notifier { get; set; }

        [JsonProperty("person")]
        public Person Person { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("request")]
        public Request Request { get; set; }

        [JsonProperty("server")]
        public Server Server { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        public Data()
        {
            Body = new Body();
        }
    }
}