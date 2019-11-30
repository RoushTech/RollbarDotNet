namespace RollbarDotNet.Payloads
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class Client
    {
        [JsonProperty("keys")]
        public Dictionary<string, object> Keys { get; set; }

        public Client()
        {
            Keys = new Dictionary<string, object>();
        }
    }
}