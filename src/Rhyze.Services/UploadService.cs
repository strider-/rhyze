using Rhyze.Core.Interfaces;
using Rhyze.Core.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyze.Services
{
    public class UploadService : IUploadService
    {
        private string[] AllowedAudioContentTypes = new[] { "audio/mpeg", "audio/flac", "audio/x-flac" };

        public async Task<Error> UploadTrackAsync(Guid ownerId, string contentType, Stream data)
        {
            var acceptable = AllowedAudioContentTypes.Contains(contentType);

            if (!acceptable)
            {
                return new Error($"Content type of '{contentType}' is not supported.");
            }

            return null;
        }

        public Task<Error> UploadArtworkAsync()
        {
            throw new NotImplementedException();
        }
    }
}