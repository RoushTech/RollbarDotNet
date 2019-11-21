namespace RollbarDotNet.Builder
{
    using System;
    using Abstractions;
    using Payloads;

    public class EnvironmentBuilder : IBuilder
    {
        protected IDateTime DateTime { get; }

        public EnvironmentBuilder(IDateTime datetime)
        {
            DateTime = datetime;
        }

        protected static long ConvertToUnixTime(DateTime datetime)
        {
            var sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(datetime - sTime).TotalSeconds;
        }

        public void Execute(Payload payload)
        {
            payload.Data.Timestamp = ConvertToUnixTime(DateTime.UtcNow);
            payload.Data.Language = "C#";
            payload.Data.Platform = ".NET Core";
        }
    }
}