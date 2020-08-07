using Rhyze.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Rhyze.Core.Interfaces
{
    public interface IBlobStore
    {
        Task<Error> UploadAsync(string blobPath, Stream blob, string contentType, IDictionary<string, string> metadata = null);

        Task<bool> ExistsAsync(string blobPath);

        Task<Error> DeleteAsync(string blobPath);
    }
}
