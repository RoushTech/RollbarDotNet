namespace RollbarDotNet.Payloads
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class TraceContext
    {
        [JsonProperty("post")]
        public string[] Post { get; set; }

        [JsonProperty("pre")]
        public string[] Pre { get; set; }
    }
}