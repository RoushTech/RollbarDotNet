namespace RollbarDotNet.Payloads
{
    using Newtonsoft.Json;
    
    [JsonObject(MemberSerialization.OptIn)]
    public class CrashReport
    {
        [JsonProperty("raw")]
        public string Raw { get; set; }
    }
}
