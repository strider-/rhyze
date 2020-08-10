using MediatR;
using Rhyze.API.Models;
using Rhyze.Core.Models;
using Rhyze.Data;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Queries
{
    public class GetAlbumMetadataQuery : RequireAnOwner, IRequest<AlbumMetadata>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You need to provide an album id!")]
        public AlbumId AlbumId { get; set; }
    }

    public class GetAlbumMetadataQueryHandler : IRequestHandler<GetAlbumMetadataQuery, AlbumMetadata>
    {
        private readonly IDatabase _db;

        public GetAlbumMetadataQueryHandler(IDatabase db) => _db = db;

        public async Task<AlbumMetadata> Handle(GetAlbumMetadataQuery request, CancellationToken cancellationToken)
        {
            return await _db.ExecuteAsync(new Data.Queries.GetAlbumMetadataQuery(request.OwnerId, request.AlbumId));
        }
    }
}