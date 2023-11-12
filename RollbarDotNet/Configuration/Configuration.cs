namespace RollbarDotNet.Configuration
{
    using Newtonsoft.Json;

    public class Configuration
    {
        public JsonSerializerSettings JsonSettings => new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}