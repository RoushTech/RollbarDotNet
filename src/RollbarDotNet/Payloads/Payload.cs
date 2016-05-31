namespace RollbarDotNet.Payloads
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class Payload
    {
        public Payload()
        {
            this.Data = new Data();
        }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        
        [JsonProperty("data")]
        public Data Data { get; set; }
    }
}
