namespace RollbarDotNet.Builder
{
    using Payloads;
    using System.Reflection;
    using System.Diagnostics;

    public class NotifierBuilder : IBuilder
    {
        public void Execute(Payload payload)
        {
            payload.Data.Notifier = new Notifier();
            payload.Data.Notifier.Name = "RollbarDotNet";
            var version = FileVersionInfo.GetVersionInfo(typeof(Rollbar).GetTypeInfo().Assembly.Location).ProductVersion;
            payload.Data.Notifier.Version = version;
        }
    }
}
