using System;

namespace Rhyze.Core.Models
{
    /// <summary>
    /// Information and metadata about a song uploaded by a user
    /// </summary>
    public class Track
    {
        /// <summary>
        /// The unique identifier of this track
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The title of this track
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The name of the album the track belongs to
        /// </summary>
        public string Album { get; set; }

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
        public int TrackNo { get; set; }

        /// <summary>
        /// The total number of tracks on the album
        /// </summary>
        public int TrackCount { get; set; }

        /// <summary>
        /// The disc number of this track 
        /// </summary>
        public int DiscNo { get; set; }

        /// <summary>
        /// The total number of discs in the album
        /// </summary>
        public int DiscCount { get; set; }

        /// <summary>
        /// The year the track was produced
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// The length of the track in seconds
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// The url of the artwork of this track
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The url of this track
        /// </summary>
        public string AudioUrl { get; set; }

        /// <summary>
        /// The date and time this track was uploaded
        /// </summary>
        public DateTime UploadedUtc { get; set; }

        /// <summary>
        /// The last date and time this track was played
        /// </summary>
        public DateTime? LastPlayedUtc { get; set; }

        /// <summary>
        /// The number of times this track has been played
        /// </summary>
        public int PlayCount { get; set; }

        /// <summary>
        /// The unique id of the user who uploaded this track
        /// </summary>
        public Guid OwnerId { get; set; }
    }
}