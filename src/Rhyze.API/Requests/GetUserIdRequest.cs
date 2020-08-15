using MediatR;
using Rhyze.API.Extensions;
using Rhyze.Data;
using Rhyze.Data.Queries;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Requests
{
    public class GetUserIdRequest : IRequest<Guid>
    {
        public GetUserIdRequest(ClaimsPrincipal user) => User = user;

        public ClaimsPrincipal User { get; }
    }

    public class GetUserIdRequestHandler : IRequestHandler<GetUserIdRequest, Guid>
    {
        private readonly IDatabase _db;

        public GetUserIdRequestHandler(IDatabase db) => _db = db;

        public async Task<Guid> Handle(GetUserIdRequest request, CancellationToken cancellationToken)
        {
            var query = new GetUserIdFromIdentityQuery(request.User.IdentityId(), request.User.Email());

            var id = await _db.ExecuteAsync(query);

            return id;
        }
    }
}