namespace RollbarDotNet.Payloads
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    [JsonObject(MemberSerialization.OptIn)]
    public class Client
    {
        public Client()
        {
            this.Keys = new Dictionary<string, object>();
        }

        [JsonProperty("keys")]
        Dictionary<string, object> Keys { get; set; }
    }
}
