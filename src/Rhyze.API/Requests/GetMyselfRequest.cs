using MediatR;
using Rhyze.API.Extensions;
using Rhyze.API.Models;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Requests
{
    public class GetMyselfRequest : IRequest<Me>
    {
        public GetMyselfRequest(ClaimsPrincipal user) => User = user;

        public ClaimsPrincipal User { get; }
    }

    public class GetMyselfRequestHandler : IRequestHandler<GetMyselfRequest, Me>
    {
        public Task<Me> Handle(GetMyselfRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Me
            {
                Email = request.User.Email(),
                UserId = request.User.UserId()
            });
        }
    }
}