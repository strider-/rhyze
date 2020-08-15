using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Rhyze.API.Requests;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rhyze.API.Security
{
    public class RhyzeBearerEvents : JwtBearerEvents
    {
        private readonly IMediator _mediator;

        public RhyzeBearerEvents(IMediator mediator) => _mediator = mediator;

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            var id = await _mediator.Send(new GetUserIdRequest(context.Principal));

            var ident = (ClaimsIdentity)context.Principal.Identity;

            ident.AddClaim(new Claim("rhyze_id", id.ToString()));

            await base.TokenValidated(context);
        }
    }
}