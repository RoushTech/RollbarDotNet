namespace RollbarDotNet.Payloads
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class Payload
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        public Payload()
        {
            Data = new Data();
        }
    }
}