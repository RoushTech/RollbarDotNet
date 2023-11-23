namespace RollbarDotNet
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Configuration;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Payloads;

    public class RollbarClient
    {
        public Configuration.Configuration Configuration { get; }

        protected RollbarOptions RollbarOptions { get; }

        protected Uri RollbarUri => new Uri("https://api.rollbar.com/api/1/item/");

        protected IHttpClientFactory HttpClientFactory { get; }

        public RollbarClient(IOptions<RollbarOptions> rollbarOptions, IHttpClientFactory httpClientFactory)
        {
            Configuration = new Configuration.Configuration();
            RollbarOptions = rollbarOptions.Value;
            HttpClientFactory = httpClientFactory;
        }

        public virtual async Task<Response> Send(Payload payload)
        {
            if (RollbarOptions.Disabled)
            {
                return new Response
                {
                    Result = new Result
                    {
                        Uuid = null
                    }
                };
            }

            var json = Serialize(payload);
            using (var httpClient = HttpClientFactory.CreateClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.PostAsync(RollbarUri, new JsonHttpContentSerializer(json));
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(response.ToString());
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Response>(responseJson);
            }
        }

        protected string Serialize(Payload payload)
        {
            return JsonConvert.SerializeObject(payload, Configuration.JsonSettings);
        }
    }
}