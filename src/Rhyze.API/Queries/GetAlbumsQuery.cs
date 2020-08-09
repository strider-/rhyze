using MediatR;
using Rhyze.Core.Models;
using Rhyze.Data;
using Rhyze.Data.Queries;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Queries
{
    public class GetAlbumsQuery : IRequest<IEnumerable<Album>>
    {
        public GetAlbumsQuery(Guid ownerId, int skip, int take)
        {
            OwnerId = ownerId;
            Skip = Math.Max(0, skip);
            Take = Math.Max(0, take);
        }

        public Guid OwnerId { get; }

        public int Skip { get; }

        public int Take { get; }
    }

    public class GetAlbumsQueryHandler : IRequestHandler<GetAlbumsQuery, IEnumerable<Album>>
    {
        private readonly IDatabase _db;

        public GetAlbumsQueryHandler(IDatabase db) => _db = db;

        public async Task<IEnumerable<Album>> Handle(GetAlbumsQuery request, CancellationToken cancellationToken)
        {
            return await _db.ExecuteAsync(new GetAlbumListingQuery(request.OwnerId, request.Skip, request.Take));
        }
    }
}
