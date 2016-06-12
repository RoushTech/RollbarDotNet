namespace RollbarDotNet.Tests.Builder
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.Extensions.Primitives;
    using Moq;
    using Payloads;
    using RollbarDotNet.Builder;
    using System.Collections.Generic;
    using System.Net;
    using Xunit;

    public class RequestBuilderTests
    {
        protected Dictionary<string, string> DefaultDictionary = new Dictionary<string, string>
        {
            { "blacklist", "test" },
            { "test", "test" }
        };

        [Fact]
        public void Builds_Payload_Url()
        {
            var payload = this.GeneratePayload();
            Assert.Equal("http://my.test.domain/request/url/here.txt", payload.Data?.Request?.Url);
        }

        [Fact]
        public void Builds_Payload_Method()
        {
            var payload = this.GeneratePayload();
            Assert.Equal("GET", payload.Data?.Request?.Method);
        }

        [Fact]
        public void Builds_Payload_Headers()
        {
            var headers = this.GeneratePayload()?.Data?.Request?.Headers;
            Assert.Equal(this.DefaultDictionary, headers);
        }

        [Fact]
        public void Blacklists_Payload_Headers()
        {
            var headers = this.GeneratePayload(enableBlacklist: true)?.Data?.Request?.Headers;
            Assert.True(headers.ContainsKey("blacklist"));
            Assert.Equal("**********", headers["blacklist"]);
        }

        [Fact]
        public void Builds_Payload_UserIp()
        {
            var payload = this.GeneratePayload();
            Assert.Equal(IPAddress.Loopback.ToString(), payload.Data?.Request?.UserIp);
        }

        [Fact]
        public void Builds_Payload_Cookies()
        {
            var query = this.GeneratePayload()?.Data?.Request?.Cookies;
            Assert.Equal(this.DefaultDictionary, query);
        }

        [Fact]
        public void Blacklists_Payload_Cookies()
        {
            var cookies = this.GeneratePayload(enableBlacklist: true)?.Data?.Request?.Cookies;
            Assert.True(cookies.ContainsKey("blacklist"));
            Assert.Equal("**********", cookies["blacklist"]);
        }

        [Fact]
        public void Builds_Payload_Query_Get()
        {
            var query = this.GeneratePayload()?.Data?.Request?.Get;
            Assert.Equal(this.DefaultDictionary, query);
        }

        [Fact]
        public void Blacklists_Payload_Query_Get()
        {
            var query = this.GeneratePayload(enableBlacklist: true)?.Data?.Request?.Get;
            Assert.True(query.ContainsKey("blacklist"));
            Assert.Equal("**********", query["blacklist"]);
        }

        [Fact]
        public void Builds_Payload_Query_Post()
        {
            var query = this.GeneratePayload(false, "POST")?.Data?.Request?.Post;
            Assert.Equal(this.DefaultDictionary, query);
        }

        [Fact]
        public void Blacklists_Payload_Query_Post()
        {
            var query = this.GeneratePayload(enableBlacklist: true, method: "POST")?.Data?.Request?.Post;
            Assert.True(query.ContainsKey("blacklist"));
            Assert.Equal("**********", query["blacklist"]);
        }

        [Fact]
        public void Builds_Payload_QueryString()
        {
            var queryString = this.GeneratePayload()?.Data?.Request?.QueryString;
            Assert.Equal("?query=test&blacklist=here", queryString);
        }

        [Fact]
        public void Blacklists_Payload_QueryString()
        {
            var queryString = this.GeneratePayload(enableBlacklist: true)?.Data?.Request?.QueryString;
            Assert.Equal("?query=test&blacklist=**********", queryString);
        }

        protected Payload GeneratePayload(bool enableBlacklist = false, string method = "GET")
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.Scheme).Returns("http");
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.Host).Returns(new HostString("my.test.domain"));
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.Path).Returns("/request/url/here.txt");
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.Method).Returns(method);
            httpContextAccessorMock.Setup(h => h.HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress).Returns(IPAddress.Loopback);
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.Headers).Returns(this.GenerateHeaderDictionary);
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.Query).Returns(this.GenerateQueryCollection);
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.Form).Returns(this.GenerateFormCollection);
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.Cookies).Returns(this.GenerateCookieCollection);
            httpContextAccessorMock.Setup(h => h.HttpContext.Request.QueryString).Returns(new QueryString("?query=test&blacklist=here"));

            var blacklistCollectionMock = new Mock<IBlacklistCollection>();
            blacklistCollectionMock.Setup(b => b.Check("test")).Returns(false);
            blacklistCollectionMock.Setup(b => b.Check("blacklist")).Returns(enableBlacklist);

            var requestBuilder = new RequestBuilder(blacklistCollectionMock.Object, httpContextAccessorMock.Object);
            var payload = new Payload();
            requestBuilder.Execute(payload);
            return payload;
        }

        protected IHeaderDictionary GenerateHeaderDictionary()
        {
            var headerDictionaryMock = new Mock<IHeaderDictionary>();
            headerDictionaryMock
                .Setup(h => h.GetEnumerator())
                .Returns(this.GenerateStringValueEnumerator());
            return headerDictionaryMock.Object;
        }

        protected IRequestCookieCollection GenerateCookieCollection()
        {
            var cookieCollectionMock = new Mock<IRequestCookieCollection>();
            cookieCollectionMock.Setup(h => h.GetEnumerator())
                .Returns(this.GenerateStringEnumerator());
            return cookieCollectionMock.Object;
        }

        protected IQueryCollection GenerateQueryCollection()
        {
            var queryCollectionMock = new Mock<IQueryCollection>();
            queryCollectionMock.Setup(h => h.GetEnumerator())
                .Returns(this.GenerateStringValueEnumerator());
            return queryCollectionMock.Object;
        }

        protected IFormCollection GenerateFormCollection()
        {
            var formCollectionMock = new Mock<IFormCollection>();
            formCollectionMock.Setup(h => h.GetEnumerator())
                .Returns(this.GenerateStringValueEnumerator());
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
    }
}
