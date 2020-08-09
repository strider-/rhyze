using Rhyze.Core.Interfaces;
using Rhyze.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Rhyze.Services
{
    public class UploadService : IUploadService, IDisposable
    {
        private readonly IBlobStore _store;
        private readonly HMAC _hasher;

        private readonly string[] AllowedAudioContentTypes = new[] { "audio/mpeg", "audio/flac", "audio/x-flac" };

        public UploadService(IBlobStore store, HMAC hasher = null)
        {
            _store = store;
            _hasher = hasher ?? HMAC.Create(nameof(HMACMD5));
        }

        public async Task<Error> UploadTrackAsync(Guid ownerId, string contentType, Stream data)
        {
            if (!AllowedAudioContentTypes.Contains(contentType))
            {
                return new Error($"Content type of '{contentType}' is not supported.");
            }

            if(ownerId == Guid.Empty)
            {
                return new Error("The owner id cannot be an empty guid.");
            }

            // ensuring unique uploads on a user basis
            _hasher.Key = ownerId.ToByteArray();

            var md5 = _hasher.ComputeHash(data).ToHexString();
            var path = GenerateBlobPath("audio", md5);

            data.Seek(0, SeekOrigin.Begin);

            var metadata = new Dictionary<string, string>
            {
                { "ownerId", ownerId.ToString() }
            };

            return await _store.UploadAsync(path, data, contentType, metadata);
        }

        public Task<Error> UploadArtworkAsync()
        {
            throw new NotImplementedException();
        }

        private string GenerateBlobPath(string prefix, string md5)
        {
            return $"{prefix}/{md5[0..3]}/{md5[3..6]}/{md5[6..9]}/{md5}";
        }

        public void Dispose() => _hasher.Dispose();
    }
}