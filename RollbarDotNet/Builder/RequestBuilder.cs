namespace RollbarDotNet.Builder
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Payloads;

    public class RequestBuilder : IBuilder
    {
        protected IBlacklistCollection BlacklistCollection { get; }

        protected IHttpContextAccessor ContextAccessor { get; }

        public RequestBuilder(
            IBlacklistCollection blacklistCollection,
            IHttpContextAccessor contextAccessor)
        {
            BlacklistCollection = blacklistCollection;
            ContextAccessor = contextAccessor;
        }

        public void Execute(Payload payload)
        {
            payload.Data.Request = new Request();
            BuildRequest(payload.Data.Request);
        }

        protected void BuildRequest(Request request)
        {
            var context = ContextAccessor.HttpContext;
            if (context == null)
            {
                return;
            }

            request.Url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}";
            request.Method = context.Request.Method.ToUpper();
            request.Headers = HeadersToDictionary(context.Request.Headers);
            request.UserIp = context.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString();
            request.Cookies = CookiesToDictionary(context.Request.Cookies);

            if (context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                request.Get = QueryToDictionary(context.Request.Query);
            }
            else if (context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase)
                     && context.Request.HasFormContentType)
            {
                request.Post = FormToDictionary(context.Request.Form);
            }

            if (context.Request.QueryString.HasValue)
            {
                request.QueryString = QueryStringBreakdown(context.Request.QueryString.Value);
            }
        }

        protected string QueryStringBreakdown(string queryString)
        {
            if (queryString.Length == 0)
            {
                return queryString;
            }

            var questionMarkRemoved = false;
            if (queryString[0] == '?')
            {
                questionMarkRemoved = true;
                queryString = queryString.Substring(1, queryString.Length - 1);
            }

            var parameters = queryString.Split('&');
            for (var i = 0; i < parameters.Length; i++)
            {
                var keyValue = parameters[i].Split('=');
                if (keyValue.Length != 2)
                {
                    continue;
                }

                var tempString = keyValue[0] + "=";
                tempString += BlacklistCollection.Check(keyValue[0]) ? "**********" : keyValue[1];
                parameters[i] = tempString;
            }

            var newQueryString = string.Join("&", parameters);
            return questionMarkRemoved ? $"?{newQueryString}" : newQueryString;
        }

        protected Dictionary<string, string> CookiesToDictionary(IRequestCookieCollection cookieCollection)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var cookie in cookieCollection)
            {
                dictionary.Add(cookie.Key, BlacklistCollection.Check(cookie.Key) ? "**********" : cookie.Value);
            }

            return dictionary.Count == 0 ? null : dictionary;
        }

        protected Dictionary<string, string> FormToDictionary(IFormCollection formCollection)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var form in formCollection)
            {
                dictionary.Add(form.Key, BlacklistCollection.Check(form.Key) ? "**********" : (string)form.Value);
            }

            return dictionary.Count == 0 ? null : dictionary;
        }

        protected Dictionary<string, string> QueryToDictionary(IQueryCollection queryCollection)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var query in queryCollection)
            {
                dictionary.Add(query.Key,
                    BlacklistCollection.Check(query.Key) ? "**********" : (string)query.Value);
            }

            return dictionary.Count == 0 ? null : dictionary;
        }

        protected Dictionary<string, string> HeadersToDictionary(IHeaderDictionary headerDictionary)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var header in headerDictionary)
            {
                dictionary.Add(header.Key,
                    BlacklistCollection.Check(header.Key) ? "**********" : (string)header.Value);
            }

            return dictionary.Count == 0 ? null : dictionary;
        }
    }
}