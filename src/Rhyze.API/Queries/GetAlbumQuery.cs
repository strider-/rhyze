using MediatR;
using Rhyze.API.Models;
using Rhyze.Core.Models;
using Rhyze.Data;
using Rhyze.Data.Queries;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Commands
{
    public class GetAlbumQuery : RequireAnOwner, IRequest<IEnumerable<TrackVM>>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You need to provide an album name!")]
        public string Name { get; set; }
    }

    public class GetAlbumQueryHandler : IRequestHandler<GetAlbumQuery, IEnumerable<TrackVM>>
    {
        private readonly IDatabase _db;

        public GetAlbumQueryHandler(IDatabase db) => _db = db;

        public async Task<IEnumerable<TrackVM>> Handle(GetAlbumQuery request, CancellationToken cancellationToken)
        {
            var tracks = await _db.ExecuteAsync(new GetAlbumByNameQuery(request.OwnerId, request.Name));

            return ToViewModel(tracks);
        }

        private IEnumerable<TrackVM> ToViewModel(IEnumerable<Track> tracks) =>
            tracks.Select(t => new TrackVM
            {
                Id = t.Id,
                Album = t.Album,
                AlbumArtist = t.AlbumArtist,
                Artist = t.Artist,
                DiscCount = t.DiscCount,
                DiscNo = t.DiscNo,
                Duration = t.Duration,
                ImageUrl = t.ImageUrl,
                PlayCount = t.PlayCount,
                Title = t.Title,
                TrackCount = t.TrackCount,
                TrackNo = t.TrackNo,
                Year = t.Year
            });
    }
}