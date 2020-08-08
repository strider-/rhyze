using System.IO;

namespace Rhyze.Core.Models
{
    public class AudioTag
    {
        public double Duration { get; set; }

        public string Album { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string AlbumArtist { get; set; }

        public int TrackNumber { get; set; }

        public int TotalTracks { get; set; }

        public int DiscNumber { get; set; }

        public int TotalDiscs { get; set; }

        public int Year { get; set; }

        public Stream Artwork { get; set; }

        public string ArtworkMimeType { get; set; }

        public bool HasArtwork => Artwork != null && !string.IsNullOrWhiteSpace(ArtworkMimeType);
    }
}