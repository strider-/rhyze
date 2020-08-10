using System;

namespace Rhyze.Core.Models
{
    /// <summary>
    /// Represents a collection of tracks by their album name
    /// </summary>
    public class Album
    {
        private string _name;
        private string _artist;

        public AlbumId Id { get; private set; }
        /// <summary>
        /// The name of the album
        /// </summary>
        public string Name { get => _name; set { _name = value; GenerateAlbumId(); } }

        /// <summary>
        /// The album artist(s)
        /// </summary>
        public string Artist { get => _artist; set { _artist = value; GenerateAlbumId(); } }

        /// <summary>
        /// The url of the artwork for this album
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The most recent date that any track in this album was played or uploaded.
        /// </summary>
        public DateTime TouchedUtc { get; set; }

        private void GenerateAlbumId()
        {
            if (string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_artist))
            {
                Id = null;
            }
            else
            {
                Id = new AlbumId(_artist, _name);
            }
        }
    }
}