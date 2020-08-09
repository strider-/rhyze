using Rhyze.Core.Models;
using System.IO;
using System.Threading.Tasks;

namespace Rhyze.Core.Interfaces
{
    /// <summary>
    /// Contract for reading audio file metadata
    /// </summary>
    public interface ITagReader
    {
        /// <summary>
        /// Reads the metadata of a given audio stream
        /// </summary>
        /// <param name="stream">The audio stream to read tags out of</param>
        /// <param name="contentType">The content type of the audio stream</param>
        /// <returns></returns>
        Task<AudioTag> ReadTagAsync(Stream stream, string contentType);
    }
}
