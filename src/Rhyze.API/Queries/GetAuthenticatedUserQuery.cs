using MediatR;
using Rhyze.API.Models;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Queries
{
    public class GetAuthenticatedUserQuery : IRequest<AuthenticatedUser>
    {
        public GetAuthenticatedUserQuery(ClaimsPrincipal user) => User = user;

        public ClaimsPrincipal User { get; }
    }

    public class GetAuthenticatedUserQueryHandler : IRequestHandler<GetAuthenticatedUserQuery, AuthenticatedUser>
    {
        public Task<AuthenticatedUser> Handle(GetAuthenticatedUserQuery request, CancellationToken cancellationToken)
        {
            var email = request.User.FindFirstValue(ClaimTypes.Email);
            var id = request.User.FindFirstValue("user_id");

            return Task.FromResult(new AuthenticatedUser
            {
                Email = email,
                UserId = id
            });
        }
    }
}