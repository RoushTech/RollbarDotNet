namespace RollbarDotNet.Payloads
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class Response
    {
        [JsonProperty("err")]
        public int Error { get; set; }

        [JsonProperty("result")]
        public Result Result { get; set; }
    }
}