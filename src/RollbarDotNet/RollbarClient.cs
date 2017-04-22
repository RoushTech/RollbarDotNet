namespace RollbarDotNet
{
    using Configuration;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Payloads;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class RollbarClient
    {
        public RollbarClient(IOptions<RollbarOptions> rollbarOptions)
        {
            this.Configuration = new Configuration.Configuration();
            this.RollbarOptions = rollbarOptions.Value;
        }

        public Configuration.Configuration Configuration { get; set; }

        protected RollbarOptions RollbarOptions { get; set; }

        protected Uri RollbarUri { get { return new Uri("https://api.rollbar.com/api/1/item/"); } }

        public async Task<Response> Send(Payload payload)
        {
            if (this.RollbarOptions.Disabled)
            {
                return new Response
                {
                    Result = new Result
                    {
                        Uuid = null
                    }
                };
            }

            string json = this.Serialize(payload);
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.PostAsync(this.RollbarUri, new JsonHttpContentSerializer(json));
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(response.ToString());
                }
                else
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Response>(responseJson);
                }
            }
        }

        protected string Serialize(Payload payload)
        {
            return JsonConvert.SerializeObject(payload, this.Configuration.JsonSettings);
        }
    }
}
