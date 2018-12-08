namespace RollbarDotNet.Payloads
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class Result
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }
    }
}