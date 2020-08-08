using System;
using System.Security.Claims;

namespace Rhyze.Tests.Fixtures
{
    public class ClaimsPrincipalFixture
    {
        public ClaimsPrincipal User { get; }

        public string ExpectedIdentityId { get; } = "ident_id";

        public string ExpectedEmail { get; } = "no@where.com";

        public Guid ExpectedRhyzeId { get; } = Guid.NewGuid();

        public ClaimsPrincipalFixture()
        {
            User = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new[] {
                        new Claim("user_id", ExpectedIdentityId),
                        new Claim(ClaimTypes.Email, ExpectedEmail)
                    }, "Test"
                )
            );
        }

        public ClaimsPrincipalFixture WithRhyzeId()
        {
            (User.Identity as ClaimsIdentity).AddClaim(new Claim("rhyze_id", ExpectedRhyzeId.ToString()));

            return this;
        }
    }
}