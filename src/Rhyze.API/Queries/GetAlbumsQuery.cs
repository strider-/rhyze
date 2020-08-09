using MediatR;
using Rhyze.Core.Models;
using Rhyze.Data;
using Rhyze.Data.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Queries
{
    public class GetAlbumsQuery : IRequest<IEnumerable<Album>>
    {
        public Guid OwnerId { get; set; }

        [Range(0, int.MaxValue)]
        public int Skip { get; set; } = 0;

        [Range(0, 1000, ErrorMessage = "You can only fetch up to 1000 albums at a time.")]
        public int Take { get; set; } = 50;
    }

    public class GetAlbumsQueryHandler : IRequestHandler<GetAlbumsQuery, IEnumerable<Album>>
    {
        private readonly IDatabase _db;

        public GetAlbumsQueryHandler(IDatabase db) => _db = db;

        public async Task<IEnumerable<Album>> Handle(GetAlbumsQuery request, CancellationToken cancellationToken)
        {
            var query = new GetAlbumListingQuery(
                request.OwnerId,
                request.Skip,
                request.Take
            );

            return await _db.ExecuteAsync(query);
        }
    }
}