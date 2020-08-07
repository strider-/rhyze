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
    public class UploadService : IUploadService
    {
        private readonly IBlobStore _store;
        private readonly HashAlgorithm _hasher;

        private string[] AllowedAudioContentTypes = new[] { "audio/mpeg", "audio/flac", "audio/x-flac" };

        public UploadService(IBlobStore store, HashAlgorithm hasher = null)
        {
            _store = store;
            _hasher = hasher ?? MD5.Create();
        }

        public async Task<Error> UploadTrackAsync(Guid ownerId, string contentType, Stream data)
        {
            if (!AllowedAudioContentTypes.Contains(contentType))
            {
                return new Error($"Content type of '{contentType}' is not supported.");
            }

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
    }
}