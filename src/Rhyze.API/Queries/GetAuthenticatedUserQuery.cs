using MediatR;
using Microsoft.AspNetCore.Http;
using Rhyze.API.Models;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Queries
{
    public class GetAuthenticatedUserQuery : IRequest<AuthenticatedUser>
    {

    }

    public class GetAuthenticatedUserQueryHandler : IRequestHandler<GetAuthenticatedUserQuery, AuthenticatedUser>
    {
        private readonly HttpContext _context;

        public GetAuthenticatedUserQueryHandler(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor.HttpContext;
        }

        public Task<AuthenticatedUser> Handle(GetAuthenticatedUserQuery request, CancellationToken cancellationToken)
        {
            var email = _context.User.FindFirstValue(ClaimTypes.Email);
            var id = _context.User.FindFirstValue("user_id");

            return Task.FromResult(new AuthenticatedUser
            {
                Email = email,
                UserId = id
            });
        }
    }
}