namespace RollbarDotNet.Payloads
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class Body
    {
        [JsonProperty("crash_report")]
        public CrashReport CrashReport { get; set; }

        [JsonProperty("message")]
        public Message Message { get; set; }

        [JsonProperty("trace")]
        public Trace Trace { get; set; }

        [JsonProperty("trace_chain")]
        public List<Trace> TraceChain { get; set; }
    }
}