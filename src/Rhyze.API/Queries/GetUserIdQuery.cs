using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Queries
{
    public class GetUserIdQuery : IRequest<Guid>
    {
        public GetUserIdQuery(string identityId) => IdentityId = identityId;

        public string IdentityId { get; }
    }

    public class GetUserIdQueryHandler : IRequestHandler<GetUserIdQuery, Guid>
    {
        public Task<Guid> Handle(GetUserIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Guid.NewGuid());
        }
    }
}
