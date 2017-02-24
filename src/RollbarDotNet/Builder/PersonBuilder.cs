using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RollbarDotNet.Payloads;

namespace RollbarDotNet.Builder
{
    public class PersonBuilder : IBuilder
    {
        private readonly IHttpContextAccessor contextAccessor;

        public PersonBuilder(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public void Execute(Payload payload)
        {
            payload.Data.Person = new Person();
            BuildPerson(payload.Data.Person);
        }

        private void BuildPerson(Person person)
        {
            var principal = contextAccessor.HttpContext?.User;
            if (principal == null)
            {
                return;
            }

            person.Username = principal.Identity.Name;
            person.Email = principal.FindFirst(claim => claim.Type == ClaimTypes.Email)?.Value;
            person.Id = principal.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}