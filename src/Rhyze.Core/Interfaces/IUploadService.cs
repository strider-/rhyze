using Rhyze.Core.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Rhyze.Core.Interfaces
{
    /// <summary>
    /// Contract for uploading audio files and artwork metadata
    /// </summary>
    public interface IUploadService
    {
        /// <summary>
        /// Uploads an audio track
        /// </summary>
        /// <param name="ownerId">The unique identifier of the user who uploaded the track</param>
        /// <param name="contentType">The content type of the audio stream</param>
        /// <param name="data">The audio stream to upload</param>
        /// <returns></returns>
        Task<Error> UploadTrackAsync(Guid ownerId, string contentType, Stream data);

        /// <summary>
        /// TBD
        /// </summary>
        /// <returns></returns>
        Task<Error> UploadArtworkAsync();
    }
}
