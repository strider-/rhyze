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
        /// <param name="albumName"></param>
        /// <returns></returns>
        Task EnqueueAlbumDeletion(string albumName);
    }
}
