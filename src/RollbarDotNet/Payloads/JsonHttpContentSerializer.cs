namespace RollbarDotNet.Payloads
{
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class JsonHttpContentSerializer : HttpContent
    {
        protected byte[] Payload { get; set; }

        public JsonHttpContentSerializer(string json)
        {
            this.Payload = Encoding.UTF8.GetBytes(json);
        }

        protected async override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            await stream.WriteAsync(this.Payload, 0, this.Payload.Length);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = this.Payload.Length;
            return true;
        }
    }
}
