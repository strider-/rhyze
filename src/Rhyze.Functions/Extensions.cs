using Rhyze.Core.Models;
using System;

namespace Rhyze.Functions
{
    public static class Extensions
    {
        public static Track ToTrack(this AudioTag tag, Guid ownerId, string audioUrl, string imageUrl = null)
        {
            return new Track
            {
                Album = tag.Album,
                AlbumArtist = tag.AlbumArtist,
                AudioUrl = audioUrl,
                Artist = tag.Artist,
                DiscCount = tag.TotalDiscs,
                DiscNo = tag.DiscNumber,
                Duration = tag.Duration,
                ImageUrl = imageUrl,
                LastPlayedUtc = null,
                OwnerId = ownerId,
                PlayCount = 0,
                Title = tag.Title,
                TrackCount = tag.TotalTracks,
                TrackNo = tag.TrackNumber,
                UploadedUtc = DateTime.UtcNow,
                Year = tag.Year
            };
        }
    }
}