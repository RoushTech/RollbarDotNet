namespace RollbarDotNet.Payloads
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    public class Trace
    {
        [JsonProperty("exception")]
        public Exception Exception { get; set; }

        [JsonProperty("frames")]
        public List<Frame> Frames { get; set; }

        public Trace()
        {
            Frames = new List<Frame>();
            Exception = new Exception();
        }
    }
}