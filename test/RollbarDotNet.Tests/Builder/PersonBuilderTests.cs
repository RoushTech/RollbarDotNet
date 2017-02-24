using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using RollbarDotNet.Builder;
using RollbarDotNet.Payloads;
using Xunit;

namespace RollbarDotNet.Tests.Builder
{
    public class PersonBuilderTests
    {
        private readonly Mock<IHttpContextAccessor> contextAccessor;
        private readonly Payload payload;
        private readonly PersonBuilder sut;

        public PersonBuilderTests()
        {
            contextAccessor = new Mock<IHttpContextAccessor>();
            sut = new PersonBuilder(contextAccessor.Object);
            payload = new Payload();
        }

        [Fact]
        public void Execute_NoPrincipal_ShouldSetEmptyPerson()
        {
            contextAccessor.Setup(accessor => accessor.HttpContext.User).Returns((ClaimsPrincipal)null);

            sut.Execute(payload);

            var person = payload.Data.Person;
            Assert.Equal(true, string.IsNullOrEmpty(person.Id));
            Assert.Equal(true, string.IsNullOrEmpty(person.Email));
            Assert.Equal(true, string.IsNullOrEmpty(person.Username));
        }

        [Fact]
        public void Execute_PrincipalWithName_ShouldSetName()
        {
            const string expected = "name";
            contextAccessor.Setup(accessor => accessor.HttpContext.User.Identity.Name).Returns(expected);

            sut.Execute(payload);

            var person = payload.Data.Person;
            Assert.Equal(expected, person.Username);
        }

        [Fact]
        public void Execute_PrincipalWithId_ShouldSetId()
        {
            const string expected = "id";
            contextAccessor.Setup(accessor => accessor.HttpContext.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, expected)
                })));

            sut.Execute(payload);

            var person = payload.Data.Person;
            Assert.Equal(expected, person.Id);
        }

        [Fact]
        public void Execute_PrincipalWithoutId_ShouldSetIdToEmpty()
        {
            contextAccessor.Setup(accessor => accessor.HttpContext.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity()));

            sut.Execute(payload);

            var person = payload.Data.Person;
            Assert.Equal(true, string.IsNullOrEmpty(person.Id));
        }

        [Fact]
        public void Execute_PrincipalWithEmail_ShouldSetEmail()
        {
            const string expected = "email";
            contextAccessor.Setup(accessor => accessor.HttpContext.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, expected)
                })));

            sut.Execute(payload);

            var person = payload.Data.Person;
            Assert.Equal(expected, person.Email);
        }

        [Fact]
        public void Execute_PrincipalWithoutEmail_ShouldSetEmailToEmpty()
        {
            contextAccessor.Setup(accessor => accessor.HttpContext.User)
                .Returns(new ClaimsPrincipal(new ClaimsIdentity()));

            sut.Execute(payload);

            var person = payload.Data.Person;
            Assert.Equal(true, string.IsNullOrEmpty(person.Email));
        }
    }
}