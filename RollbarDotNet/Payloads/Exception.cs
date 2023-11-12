namespace RollbarDotNet.Payloads
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class Exception
    {
        [JsonProperty("class")]
        public string Class { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}