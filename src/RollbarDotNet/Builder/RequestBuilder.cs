namespace RollbarDotNet.Builder
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Payloads;
    using System;
    using System.Collections.Generic;

    public class RequestBuilder : IBuilder
    {
        public RequestBuilder(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        protected readonly IHttpContextAccessor contextAccessor;

        public void Execute(Payload payload)
        {
            payload.Data.Request = new Request();
            this.BuildRequest(payload.Data.Request);
        }
        
        protected void BuildRequest(Request request)
        {
            var context = this.contextAccessor.HttpContext;
            request.Url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}";
            request.Method = context.Request.Method.ToUpper();
            request.Headers = this.HeadersToDictionary(context.Request.Headers);
            request.UserIp = context.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString();

            if (context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                request.Get = this.QueryToDictionary(context.Request.Query);
            }
            else if (context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                request.Post = this.FormToDictionary(context.Request.Form);
            }

            if (context.Request.QueryString.HasValue)
                request.QueryString = context.Request.QueryString.Value;
        }

        protected Dictionary<string, string> FormToDictionary(IFormCollection formCollection)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var form in formCollection)
            {
                dictionary.Add(form.Key, form.Value);
            }

            return dictionary.Count == 0 ? null : dictionary;
        }

        protected Dictionary<string, string> QueryToDictionary(IQueryCollection queryCollection)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var query in queryCollection)
            {
                dictionary.Add(query.Key, query.Value);
            }

            return dictionary.Count == 0 ? null : dictionary;
        }

        protected Dictionary<string, string> HeadersToDictionary(IHeaderDictionary headerDictionary)
        {
            var dictionary = new Dictionary<string, string>();
            foreach(var header in headerDictionary)
            {
                dictionary.Add(header.Key, header.Value);
            }

            return dictionary.Count == 0 ? null : dictionary;
        }
    }
}
