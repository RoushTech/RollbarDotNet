namespace RollbarDotNet.Tests.Builder
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Payloads;
    using RollbarDotNet.Builder;
    using Xunit;

    public class PersonBuilderTests
    {
        public PersonBuilderTests()
        {
            this._contextAccessor = new Mock<IHttpContextAccessor>();
            this._sut = new PersonBuilder(this._contextAccessor.Object);
            this._payload = new Payload();
        }

        private readonly Mock<IHttpContextAccessor> _contextAccessor;
        private readonly Payload _payload;
        private readonly PersonBuilder _sut;

        [Fact]
        public void Execute_NoPrincipal_ShouldSetEmptyPerson()
        {
            this._contextAccessor.Setup(accessor => accessor.HttpContext.User).Returns((ClaimsPrincipal) null);

            this._sut.Execute(this._payload);

            var person = this._payload.Data.Person;
            Assert.Equal(true, string.IsNullOrEmpty(person.Id));
            Assert.Equal(true, string.IsNullOrEmpty(person.Email));
            Assert.Equal(true, string.IsNullOrEmpty(person.Username));
        }

        [Fact]
        public void Execute_PrincipalWithEmail_ShouldSetEmail()
        {
            const string expected = "email";
            this._contextAccessor.Setup(accessor => accessor.HttpContext.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, expected)
                })));

            this._sut.Execute(this._payload);

            var person = this._payload.Data.Person;
            Assert.Equal(expected, person.Email);
        }

        [Fact]
        public void Execute_PrincipalWithId_ShouldSetId()
        {
            const string expected = "id";
            this._contextAccessor.Setup(accessor => accessor.HttpContext.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, expected)
                })));

            this._sut.Execute(this._payload);

            var person = this._payload.Data.Person;
            Assert.Equal(expected, person.Id);
        }

        [Fact]
        public void Execute_PrincipalWithName_ShouldSetName()
        {
            const string expected = "name";
            this._contextAccessor.Setup(accessor => accessor.HttpContext.User.Identity.Name).Returns(expected);

            this._sut.Execute(this._payload);

            var person = this._payload.Data.Person;
            Assert.Equal(expected, person.Username);
        }

        [Fact]
        public void Execute_PrincipalWithoutEmail_ShouldSetEmailToEmpty()
        {
            this._contextAccessor.Setup(accessor => accessor.HttpContext.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity()));

            this._sut.Execute(this._payload);

            var person = this._payload.Data.Person;
            Assert.Equal(true, string.IsNullOrEmpty(person.Email));
        }

        [Fact]
        public void Execute_PrincipalWithoutId_ShouldSetIdToEmpty()
        {
            this._contextAccessor.Setup(accessor => accessor.HttpContext.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity()));

            this._sut.Execute(this._payload);

            var person = this._payload.Data.Person;
            Assert.Equal(true, string.IsNullOrEmpty(person.Id));
        }
    }
}