namespace RollbarDotNet.Builder
{
    using Abstractions;
    using Payloads;
    using System;

    public class EnvironmentBuilder : IBuilder
    {
        public EnvironmentBuilder(IDateTime datetime)
        {
            this.DateTime = datetime;
        }

        protected IDateTime DateTime { get; set; }

        protected static long ConvertToUnixTime(DateTime datetime)
        {
            var sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(datetime - sTime).TotalSeconds;
        }

        public void Execute(Payload payload)
        {
            payload.Data.Timestamp = ConvertToUnixTime(this.DateTime.UtcNow);
            payload.Data.Language = "C#";
            payload.Data.Platform = ".NET Core";
        }
    }
}
