using System.IO;

namespace Rhyze.Core.Models
{
    /// <summary>
    /// Represents the audio metadata of a track
    /// </summary>
    public class AudioTag
    {
        /// <summary>
        /// The length of the track in seconds
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// The name of the album the track belongs to
        /// </summary>
        public string Album { get; set; }

        /// <summary>
        /// The title of this track
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The artist(s) that composed this track
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// The artist(s) that composed the album
        /// </summary>
        public string AlbumArtist { get; set; }

        /// <summary>
        /// The ordinal position of this track on the album
        /// </summary>
        public int TrackNumber { get; set; }

        /// <summary>
        /// The total number of tracks on the album
        /// </summary>
        public int TotalTracks { get; set; }

        /// <summary>
        /// The disc number of this track 
        /// </summary>
        public int DiscNumber { get; set; }

        /// <summary>
        /// The total number of discs in the album
        /// </summary>
        public int TotalDiscs { get; set; }

        /// <summary>
        /// The year the track was produced
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// The tag cover art, if any
        /// </summary>
        public Stream Artwork { get; set; }

        /// <summary>
        /// The content type of the artwork, if any
        /// </summary>
        public string ArtworkMimeType { get; set; }

        /// <summary>
        /// Whether or not the <see cref="Artwork"/> and <see cref="ArtworkMimeType"/> were set
        /// </summary>
        public bool HasArtwork => Artwork != null && !string.IsNullOrWhiteSpace(ArtworkMimeType);
    }
}