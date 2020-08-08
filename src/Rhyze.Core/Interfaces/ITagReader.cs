using Rhyze.Core.Models;
using System.IO;
using System.Threading.Tasks;

namespace Rhyze.Core.Interfaces
{
    public interface ITagReader
    {
        Task<AudioTag> ReadTagAsync(Stream stream, string contentType);
    }
}
