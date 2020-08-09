using System;
using System.Threading.Tasks;

namespace Rhyze.Core.Interfaces
{
    /// <summary>
    /// Contract for enqueuing the processing of specific actions
    /// </summary>
    public interface IQueueService
    {
        /// <summary>
        /// Enqueues the deletion of a given album
        /// </summary>
        /// <param name="ownerId">The unique identifier of the user who uploaded the album</param>
        /// <param name="albumName">The name of the album to delete</param>
        /// <returns></returns>
        Task EnqueueAlbumDeletionAsync(Guid ownerId, string albumName);
    }
}
