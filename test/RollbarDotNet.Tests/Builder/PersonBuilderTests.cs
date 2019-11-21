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
            _contextAccessor = new Mock<IHttpContextAccessor>();
            _sut = new PersonBuilder(_contextAccessor.Object);
            _payload = new Payload();
        }

        private readonly Mock<IHttpContextAccessor> _contextAccessor;
        private readonly Payload _payload;
        private readonly PersonBuilder _sut;

        [Fact]
        public void Execute_NoPrincipal_ShouldSetEmptyPerson()
        {
            _contextAccessor.Setup(accessor => accessor.HttpContext.User).Returns((ClaimsPrincipal)null);

            _sut.Execute(_payload);

            var person = _payload.Data.Person;
            Assert.Equal(true, string.IsNullOrEmpty(person.Id));
            Assert.Equal(true, string.IsNullOrEmpty(person.Email));
            Assert.Equal(true, string.IsNullOrEmpty(person.Username));
        }

        [Fact]
        public void Execute_PrincipalWithEmail_ShouldSetEmail()
        {
            const string expected = "email";
            _contextAccessor.Setup(accessor => accessor.HttpContext.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, expected)
                })));

            _sut.Execute(_payload);

            var person = _payload.Data.Person;
            Assert.Equal(expected, person.Email);
        }

        [Fact]
        public void Execute_PrincipalWithId_ShouldSetId()
        {
            const string expected = "id";
            _contextAccessor.Setup(accessor => accessor.HttpContext.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, expected)
                })));

            _sut.Execute(_payload);

            var person = _payload.Data.Person;
            Assert.Equal(expected, person.Id);
        }

        [Fact]
        public void Execute_PrincipalWithName_ShouldSetName()
        {
            const string expected = "name";
            _contextAccessor.Setup(accessor => accessor.HttpContext.User.Identity.Name).Returns(expected);

            _sut.Execute(_payload);

            var person = _payload.Data.Person;
            Assert.Equal(expected, person.Username);
        }

        [Fact]
        public void Execute_PrincipalWithoutEmail_ShouldSetEmailToEmpty()
        {
            _contextAccessor.Setup(accessor => accessor.HttpContext.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity()));

            _sut.Execute(_payload);

            var person = _payload.Data.Person;
            Assert.Equal(true, string.IsNullOrEmpty(person.Email));
        }

        [Fact]
        public void Execute_PrincipalWithoutId_ShouldSetIdToEmpty()
        {
            _contextAccessor.Setup(accessor => accessor.HttpContext.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity()));

            _sut.Execute(_payload);

            var person = _payload.Data.Person;
            Assert.Equal(true, string.IsNullOrEmpty(person.Id));
        }
    }
}