using Rhyze.Core.Messages;
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
        /// <param name="message">The <see cref="DeleteAlbumMessage"/> containing the album name and owner</param>
        /// <returns></returns>
        Task EnqueueAlbumDeletionAsync(DeleteAlbumMessage message);

        /// <summary>
        /// Enqueues an uploaded track for metadata processing
        /// </summary>
        /// <param name="message">The <see cref="TrackUploadedMessage"/> containing the blob name</param>
        /// <returns></returns>
        Task EnqueueTrackUploadedAsync(TrackUploadedMessage message);
    }
}
