namespace RollbarDotNet.Payloads
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class Frame
    {
        [JsonProperty("argspec")]
        public string[] Argspec { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("colno")]
        public int ColumnNumber { get; set; }

        [JsonProperty("context")]
        public TraceContext Context { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("keywordspec")]
        public string Keywordspec { get; set; }

        [JsonProperty("lineno")]
        public int LineNumber { get; set; }

        [JsonProperty("locals")]
        public Dictionary<string, object> Locals { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("varargspec")]
        public string Varargspec { get; set; }

        public Frame()
        {
            this.Context = new TraceContext();
            this.Locals = new Dictionary<string, object>();
        }
    }
}