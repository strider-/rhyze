using MediatR;
using Rhyze.API.Extensions;
using Rhyze.Data;
using Rhyze.Data.Queries;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Queries
{
    public class GetUserIdQuery : IRequest<Guid>
    {
        public GetUserIdQuery(ClaimsPrincipal user) => User = user;

        public ClaimsPrincipal User { get; }
    }

    public class GetUserIdQueryHandler : IRequestHandler<GetUserIdQuery, Guid>
    {
        private readonly IDatabase _db;

        public GetUserIdQueryHandler(IDatabase db) => _db = db;

        public async Task<Guid> Handle(GetUserIdQuery request, CancellationToken cancellationToken)
        {
            var query = new GetUserIdFromIdentityQuery(request.User.IdentityId(), request.User.Email());

            var id = await _db.ExecuteAsync(query);

            return id;
        }
    }
}