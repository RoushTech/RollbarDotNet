namespace RollbarDotNet.Payloads
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class Server
    {
        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("root")]
        public string Root { get; set; }

        [JsonProperty("branch")]
        public string Branch { get; set; }

        [JsonProperty("code_version")]
        public string CodeVersion { get; set; }
    }
}
