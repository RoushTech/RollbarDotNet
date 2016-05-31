namespace RollbarDotNet.Payloads
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    [JsonObject(MemberSerialization.OptIn)]
    public class Trace
    {
        public Trace()
        {
            this.Frames = new List<Frame>();
            this.Exception = new Exception();
        }

        [JsonProperty("frames")]
        public List<Frame> Frames { get; set; }

        [JsonProperty("exception")]
        public Exception Exception { get; set; }
    }
}
