namespace RollbarDotNet.Payloads
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    [JsonObject(MemberSerialization.OptIn)]
    public class Body
    {
        public Body()
        {
            this.Message = new Message();
        }

        [JsonProperty("trace")]
        public Trace Trace { get; set; }

        [JsonProperty("trace_chain")]
        public List<Trace> TraceChain { get; set; }

        [JsonProperty("message")]
        public Message Message { get; set; }

        [JsonProperty("crash_report")]
        public CrashReport CrashReport { get; set; }
    }
}
