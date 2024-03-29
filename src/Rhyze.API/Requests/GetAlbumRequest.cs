﻿using MediatR;
using Rhyze.API.Models;
using Rhyze.Core.Models;
using Rhyze.Data;
using Rhyze.Data.Queries;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Requests
{
    public class GetAlbumRequest : RequireAnOwner, IRequest<IEnumerable<TrackVM>>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You need to provide an album id!")]
        public AlbumId AlbumId { get; set; }
    }

    public class GetAlbumRequestHandler : IRequestHandler<GetAlbumRequest, IEnumerable<TrackVM>>
    {
        private readonly IDatabase _db;

        public GetAlbumRequestHandler(IDatabase db) => _db = db;

        public async Task<IEnumerable<TrackVM>> Handle(GetAlbumRequest request, CancellationToken cancellationToken)
        {
            var tracks = await _db.ExecuteAsync(new GetAlbumByIdQuery(request.OwnerId, request.AlbumId));

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