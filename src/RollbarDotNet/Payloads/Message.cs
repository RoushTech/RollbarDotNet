namespace RollbarDotNet.Payloads
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class Message
    {
        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("route")]
        public string Route { get; set; }

        [JsonProperty("time_elapsed")]
        public decimal TimeElapsed { get; set; }
    }
}
