﻿namespace RollbarDotNet.Payloads
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
            Payload = Encoding.UTF8.GetBytes(json);
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            await stream.WriteAsync(Payload, 0, Payload.Length);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = Payload.Length;
            return true;
        }
    }
}