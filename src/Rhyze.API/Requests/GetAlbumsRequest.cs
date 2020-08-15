using MediatR;
using Rhyze.API.Models;
using Rhyze.Core.Models;
using Rhyze.Data;
using Rhyze.Data.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Requests
{
    public class GetAlbumsRequest : RequireAnOwner, IRequest<IEnumerable<Album>>
    {
        [Range(0, int.MaxValue)]
        public int Skip { get; set; } = 0;

        [Range(0, 1000, ErrorMessage = "You can only fetch up to 1000 albums at a time.")]
        public int Take { get; set; } = 50;
    }

    public class GetAlbumsRequestHandler : IRequestHandler<GetAlbumsRequest, IEnumerable<Album>>
    {
        private readonly IDatabase _db;

        public GetAlbumsRequestHandler(IDatabase db) => _db = db;

        public async Task<IEnumerable<Album>> Handle(GetAlbumsRequest request, CancellationToken cancellationToken)
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