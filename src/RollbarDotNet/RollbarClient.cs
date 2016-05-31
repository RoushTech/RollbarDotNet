namespace RollbarDotNet
{
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Payloads;
    public class RollbarClient
    {
        public RollbarClient()
        {
            this.Configuration = new Configuration.Configuration();
        }

        public Configuration.Configuration Configuration { get; set; }

        protected Uri RollbarUri { get { return new Uri("https://api.rollbar.com/api/1/item/"); } }

        public async Task<Response> Send(Payload payload)
        {
            string json = this.Serialize(payload);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.PostAsync(this.RollbarUri, new JsonHttpContentSerializer(json));
                if (!response.IsSuccessStatusCode)
                {
                    throw new System.Exception(response.ToString());
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
