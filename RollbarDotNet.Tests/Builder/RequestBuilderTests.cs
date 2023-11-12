﻿namespace RollbarDotNet.Tests.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.Extensions.Primitives;
    using Moq;
    using Payloads;
    using RollbarDotNet.Builder;
    using Xunit;

    public class RequestBuilderTests
    {
        protected Dictionary<string, string> DefaultDictionary = new Dictionary<string, string>
        {
            { "blacklist", "test" },
            { "test", "test" }
        };

        public Mock<IHttpContextAccessor> CreateIHttpContextAccessor(string method)
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.Scheme).Returns("http");
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.Host).Returns(new HostString("my.test.domain"));
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.Path).Returns("/request/url/here.txt");
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.Method).Returns(method);
            httpContextAccessorMock.Setup(h => h.HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress)
                .Returns(IPAddress.Loopback);
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.Headers).Returns(GenerateHeaderDictionary);
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.Query).Returns(GenerateQueryCollection);
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.HasFormContentType).Returns(true);
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.Form).Returns(GenerateFormCollection);
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.Cookies).Returns(GenerateCookieCollection);
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.QueryString)
                .Returns(new QueryString("?query=test&blacklist=here"));
            return httpContextAccessorMock;
        }

        protected IBlacklistCollection GenerateBacklistCollection(bool enableBlacklist = false)
        {
            var blacklistCollectionMock = new Mock<IBlacklistCollection>();
            blacklistCollectionMock.Setup(b => b.Check("test")).Returns(false);
            blacklistCollectionMock.Setup(b => b.Check("blacklist")).Returns(enableBlacklist);
            return blacklistCollectionMock.Object;
        }

        protected Payload GeneratePayload(bool enableBlacklist = false, string method = "GET")
        {
            var requestBuilder = new RequestBuilder(GenerateBacklistCollection(enableBlacklist),
                CreateIHttpContextAccessor(method).Object);
            var payload = new Payload();
            requestBuilder.Execute(payload);
            return payload;
        }

        protected IHeaderDictionary GenerateHeaderDictionary()
        {
            var headerDictionaryMock = new Mock<IHeaderDictionary>();
            headerDictionaryMock
                .Setup(h => h.GetEnumerator())
                .Returns(GenerateStringValueEnumerator());
            return headerDictionaryMock.Object;
        }

        protected IRequestCookieCollection GenerateCookieCollection()
        {
            var cookieCollectionMock = new Mock<IRequestCookieCollection>();
            cookieCollectionMock.Setup(h => h.GetEnumerator())
                .Returns(GenerateStringEnumerator());
            return cookieCollectionMock.Object;
        }

        protected IQueryCollection GenerateQueryCollection()
        {
            var queryCollectionMock = new Mock<IQueryCollection>();
            queryCollectionMock.Setup(h => h.GetEnumerator())
                .Returns(GenerateStringValueEnumerator());
            return queryCollectionMock.Object;
        }

        protected IFormCollection GenerateFormCollection()
        {
            var formCollectionMock = new Mock<IFormCollection>();
            formCollectionMock.Setup(h => h.GetEnumerator())
                .Returns(GenerateStringValueEnumerator());
            return formCollectionMock.Object;
        }

        protected IEnumerator<KeyValuePair<string, string>> GenerateStringEnumerator()
        {
            return new Dictionary<string, string>
            {
                { "blacklist", "test" },
                { "test", "test" }
            }.GetEnumerator();
        }

        protected IEnumerator<KeyValuePair<string, StringValues>> GenerateStringValueEnumerator()
        {
            return new Dictionary<string, StringValues>
            {
                { "blacklist", "test" },
                { "test", "test" }
            }.GetEnumerator();
        }

        [Fact]
        public void Blacklists_Payload_Cookies()
        {
            var cookies = GeneratePayload(true)?.Data?.Request?.Cookies;
            Assert.True(cookies.ContainsKey("blacklist"));
            Assert.Equal("**********", cookies["blacklist"]);
        }

        [Fact]
        public void Blacklists_Payload_Headers()
        {
            var headers = GeneratePayload(true)?.Data?.Request?.Headers;
            Assert.True(headers.ContainsKey("blacklist"));
            Assert.Equal("**********", headers["blacklist"]);
        }

        [Fact]
        public void Blacklists_Payload_Query_Get()
        {
            var query = GeneratePayload(true)?.Data?.Request?.Get;
            Assert.True(query.ContainsKey("blacklist"));
            Assert.Equal("**********", query["blacklist"]);
        }

        [Fact]
        public void Blacklists_Payload_Query_Post()
        {
            var query = GeneratePayload(true, "POST")?.Data?.Request?.Post;
            Assert.True(query.ContainsKey("blacklist"));
            Assert.Equal("**********", query["blacklist"]);
        }

        [Fact]
        public void Blacklists_Payload_QueryString()
        {
            var queryString = GeneratePayload(true)?.Data?.Request?.QueryString;
            Assert.Equal("?query=test&blacklist=**********", queryString);
        }

        /// <summary>
        ///     Caused when you POST a JSON payload -- Request.Form is NOT valid and the call will throw, this checks
        ///     and makes sure we're properly checking the HasFormContentType flag.
        ///     https://github.com/RoushTech/RollbarDotNet/issues/54
        /// </summary>
        [Fact]
        public void Bug_ThrowsIncorrectContentType()
        {
            var mock = CreateIHttpContextAccessor("POST");
            mock.Setup(h => h.HttpContext.Request.HasFormContentType).Returns(false);
            mock.Setup(h => h.HttpContext.Request.Form)
                .Throws(new InvalidOperationException("Incorrect Content-Type: application/json; charset=UTF-8"));
            var requestBuilder = new RequestBuilder(GenerateBacklistCollection(), mock.Object);
            var payload = new Payload();
            requestBuilder.Execute(payload);
        }

        [Fact]
        public void Builds_Payload_Cookies()
        {
            var query = GeneratePayload()?.Data?.Request?.Cookies;
            Assert.Equal(DefaultDictionary, query);
        }

        [Fact]
        public void Builds_Payload_Headers()
        {
            var headers = GeneratePayload()?.Data?.Request?.Headers;
            Assert.Equal(DefaultDictionary, headers);
        }

        [Fact]
        public void Builds_Payload_Method()
        {
            var payload = GeneratePayload();
            Assert.Equal("GET", payload.Data?.Request?.Method);
        }

        [Fact]
        public void Builds_Payload_Query_Get()
        {
            var query = GeneratePayload()?.Data?.Request?.Get;
            Assert.Equal(DefaultDictionary, query);
        }

        [Fact]
        public void Builds_Payload_Query_Post()
        {
            var query = GeneratePayload(false, "POST")?.Data?.Request?.Post;
            Assert.Equal(DefaultDictionary, query);
        }

        [Fact]
        public void Builds_Payload_QueryString()
        {
            var queryString = GeneratePayload()?.Data?.Request?.QueryString;
            Assert.Equal("?query=test&blacklist=here", queryString);
        }

        [Fact]
        public void Builds_Payload_Url()
        {
            var payload = GeneratePayload();
            Assert.Equal("http://my.test.domain/request/url/here.txt", payload.Data?.Request?.Url);
        }

        [Fact]
        public void Builds_Payload_UserIp()
        {
            var payload = GeneratePayload();
            Assert.Equal(IPAddress.Loopback.ToString(), payload.Data?.Request?.UserIp);
        }
    }
}