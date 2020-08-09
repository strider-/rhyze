using System;

namespace Rhyze.Core.Messages
{
    /// <summary>
    /// The message for enqueuing the deletion of an album
    /// </summary>
    public class DeleteAlbumMessage
    {
        /// <summary>
        /// The name of the album to delete
        /// </summary>
        public string AlbumName { get; set; }

        /// <summary>
        /// The unique id of the owner of the album
        /// </summary>
        public Guid OwnerId { get; set; }
    }
}
