using MediatR;
using Rhyze.API.Models;
using Rhyze.Core.Models;
using Rhyze.Data;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Requests
{
    public class UpdateAlbumMetadataRequest : RequireAnOwner, IRequest<AlbumMetadata>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You need to provide an album id!")]
        public AlbumId Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Album name is required!")]
        public string Album { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Album artist is required!")]
        public string AlbumArtist { get; set; }

        public int? Year { get; set; }

        public int? TrackCount { get; set; }

        public int? DiscCount { get; set; }
    }

    public class UpdateAlbumMetadataRequestHandler : IRequestHandler<UpdateAlbumMetadataRequest, AlbumMetadata>
    {
        private readonly IDatabase _db;

        public UpdateAlbumMetadataRequestHandler(IDatabase db) => _db = db;

        public async Task<AlbumMetadata> Handle(UpdateAlbumMetadataRequest request, CancellationToken cancellationToken)
        {
            var metadata = new AlbumMetadata
            {
                Id = request.Id,
                Album = request.Album,
                AlbumArtist = request.AlbumArtist,
                Year = request.Year,
                DiscCount = request.DiscCount,
                TrackCount = request.TrackCount,
            };

            var cmd = new Data.Commands.UpdateAlbumMetadataCommand(request.OwnerId, metadata);
            await _db.ExecuteAsync(cmd);

            return metadata;
        }
    }
}
