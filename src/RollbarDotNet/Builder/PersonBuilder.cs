namespace RollbarDotNet.Builder
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;
    using Payloads;

    public class PersonBuilder : IBuilder
    {
        protected IHttpContextAccessor ContextAccessor { get; }

        public PersonBuilder(IHttpContextAccessor contextAccessor)
        {
            this.ContextAccessor = contextAccessor;
        }

        public void Execute(Payload payload)
        {
            payload.Data.Person = new Person();
            this.BuildPerson(payload.Data.Person);
        }

        private void BuildPerson(Person person)
        {
            var principal = this.ContextAccessor.HttpContext?.User;
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