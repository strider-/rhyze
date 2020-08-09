using System;

namespace Rhyze.Core.Messages
{

    public class DeleteAlbumMessage
    {
        public string AlbumName { get; set; }

        public Guid OwnerId { get; set; }
    }
}
