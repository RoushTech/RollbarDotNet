namespace RollbarDotNet.Payloads
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class Message
    {
        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
