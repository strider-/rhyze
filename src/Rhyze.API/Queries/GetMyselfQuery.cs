using MediatR;
using Rhyze.API.Extensions;
using Rhyze.API.Models;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Queries
{
    public class GetMyselfQuery : IRequest<Me>
    {
        public GetMyselfQuery(ClaimsPrincipal user) => User = user;

        public ClaimsPrincipal User { get; }
    }

    public class GetMyselfHandler : IRequestHandler<GetMyselfQuery, Me>
    {
        public Task<Me> Handle(GetMyselfQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Me
            {
                Email = request.User.Email(),
                UserId = request.User.UserId()
            });
        }
    }
}