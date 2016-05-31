namespace RollbarDotNet.Payloads
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class TraceContext
    {
        [JsonProperty("pre")]
        public string[] Pre { get; set; }

        [JsonProperty("post")]
        public string[] Post { get; set; }
    }
}
