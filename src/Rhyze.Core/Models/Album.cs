using System;

namespace Rhyze.Core.Models
{
    /// <summary>
    /// Represents a collection of tracks by their album name
    /// </summary>
    public class Album
    {
        /// <summary>
        /// The name of the album
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The album artist(s)
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// The url of the artwork for this album
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The most recent date that any track in this album was played or uploaded.
        /// </summary>
        public DateTime TouchedUtc { get; set; }
    }
}
