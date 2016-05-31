namespace RollbarDotNet.Configuration
{
    using Newtonsoft.Json;

    public class Configuration
    {
        public JsonSerializerSettings JsonSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                };
            }
        }
    }
}
