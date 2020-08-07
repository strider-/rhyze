using MediatR;
using Rhyze.API.Extensions;
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
            return Task.FromResult(new AuthenticatedUser
            {
                Email = request.User.Email(),
                UserId = request.User.UserId()
            });
        }
    }
}