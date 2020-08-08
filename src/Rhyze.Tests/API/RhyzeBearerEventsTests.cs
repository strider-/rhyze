using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Moq;
using Rhyze.API.Queries;
using Rhyze.API.Security;
using Rhyze.Tests.Fixtures;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API
{
    [Trait(nameof(Rhyze.API.Security), nameof(RhyzeBearerEvents))]
    public class RhyzeBearerEventsTests
    {
        private readonly RhyzeBearerEvents _events;
        private readonly Mock<IMediator> _mediator = new Mock<IMediator>();
        private readonly ClaimsPrincipalFixture _fixture = new ClaimsPrincipalFixture();

        public RhyzeBearerEventsTests()
        {
            _events = new RhyzeBearerEvents(_mediator.Object);
        }

        [Fact]
        public async Task TokenValidated_Appends_The_Application_User_Id_Claim()
        {
            var id = Guid.NewGuid();
            var context = new TokenValidatedContext(
                new DefaultHttpContext(),
                new AuthenticationScheme(JwtBearerDefaults.AuthenticationScheme, "Jwt", typeof(JwtBearerHandler)),
                new JwtBearerOptions()
            )
            {
                Principal = _fixture.User
            };
            Predicate<Claim> predicate = c => c.Type == "rhyze_id" && c.Value == id.ToString();
            _mediator.Setup(m => m.Send(It.IsAny<GetUserIdQuery>(), default)).ReturnsAsync(id);

            Assert.False(_fixture.User.HasClaim(predicate));
            await _events.TokenValidated(context);

            Assert.True(_fixture.User.HasClaim(predicate));
        }
    }
}