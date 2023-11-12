namespace RollbarDotNet.Builder
{
    using System.Diagnostics;
    using System.Reflection;
    using Payloads;

    public class NotifierBuilder : IBuilder
    {
        public void Execute(Payload payload)
        {
            payload.Data.Notifier = new Notifier
            {
                Name = "RollbarDotNet"
            };
            var version = FileVersionInfo.GetVersionInfo(typeof(Rollbar).GetTypeInfo().Assembly.Location)
                .ProductVersion;
            payload.Data.Notifier.Version = version;
        }
    }
}