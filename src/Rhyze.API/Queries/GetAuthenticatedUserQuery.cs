using MediatR;
using Rhyze.API.Extensions;
using Rhyze.API.Models;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Queries
{
    public class GetAuthenticatedUserQuery : IRequest<Me>
    {
        public GetAuthenticatedUserQuery(ClaimsPrincipal user) => User = user;

        public ClaimsPrincipal User { get; }
    }

    public class GetAuthenticatedUserQueryHandler : IRequestHandler<GetAuthenticatedUserQuery, Me>
    {
        public Task<Me> Handle(GetAuthenticatedUserQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Me
            {
                Email = request.User.Email(),
                UserId = request.User.UserId()
            });
        }
    }
}