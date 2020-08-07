using Rhyze.Core.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Rhyze.Core.Interfaces
{
    public interface IUploadService
    {
        Task<Error> UploadTrackAsync(Guid ownerId, string contentType, Stream data);

        Task<Error> UploadArtworkAsync();
    }
}
