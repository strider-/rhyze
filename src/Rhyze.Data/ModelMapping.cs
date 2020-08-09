using RepoDb;
using Rhyze.Core.Models;

namespace Rhyze.Data
{
    static class ModelMapping
    {
        public static void Initialize()
        {
            FluentMapper.Entity<User>()
                .Table("dbo.Users")
                .Primary(u => u.Id)
                .Column(u => u.Email, "Email")
                .Column(u => u.IdentityId, "IdentityId")
                .Column(u => u.Id, "Id");

            FluentMapper.Entity<Track>()
                .Table("dbo.Tracks")
                .Primary(t => t.Id)
                .Column(t => t.Id, "Id")
                .Column(t => t.Title, "Title")
                .Column(t => t.Album, "Album")
                .Column(t => t.Artist, "Artist")
                .Column(t => t.AlbumArtist, "AlbumArtist")
                .Column(t => t.TrackNo, "TrackNo")
                .Column(t => t.TrackCount, "TrackCount")
                .Column(t => t.DiscNo, "DiscNo")
                .Column(t => t.DiscCount, "DiscCount")
                .Column(t => t.Year, "Year")
                .Column(t => t.Duration, "Duration")
                .Column(t => t.ImageUrl, "ImageUrl")
                .Column(t => t.AudioUrl, "AudioUrl")
                .Column(t => t.UploadedUtc, "UploadedUtc")
                .Column(t => t.LastPlayedUtc, "LastPlayedUtc")
                .Column(t => t.PlayCount, "PlayCount")
                .Column(t => t.OwnerId, "OwnerId");

            FluentMapper.Entity<Album>()
                .Column(a => a.Name, "Album")
                .Column(a => a.Artist, "AlbumArtist");
        }
    }
}