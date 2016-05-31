namespace RollbarDotNet.Payloads
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    [JsonObject(MemberSerialization.OptIn)]
    public class Frame
    {
        public Frame()
        {
            this.Context = new TraceContext();
            this.Locals = new Dictionary<string, object>();
        }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("lineno")]
        public int LineNumber { get; set; }

        [JsonProperty("colno")]
        public int ColumnNumber { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("context")]
        public TraceContext Context { get; set; }

        [JsonProperty("argspec")]
        public string[] Argspec { get; set; }

        [JsonProperty("varargspec")]
        public string Varargspec { get; set; }

        [JsonProperty("keywordspec")]
        public string Keywordspec { get; set; }

        [JsonProperty("locals")]
        public Dictionary<string, object> Locals { get; set; }
    }
}
