using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Rhyze.Core.Interfaces;
using Rhyze.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyze.Services
{
    public class AzureBlobStore : IBlobStore
    {
        private readonly BlobServiceClient _client;

        public AzureBlobStore(string connectionString)
        {
            _client = new BlobServiceClient(connectionString);
        }

        public AzureBlobStore(BlobServiceClient client) => _client = client;

        public Task<Error> DeleteAsync(string blobPath)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string blobPath)
        {
            throw new NotImplementedException();
        }

        public async Task<Error> UploadAsync(string blobPath, Stream blob, string contentType, IDictionary<string, string> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(contentType))
            {
                return new Error("Refusing to upload data without a content type.");
            }

            try
            {
                var (container, path) = Parse(blobPath);

                using (blob)
                {
                    var containerClient = _client.GetBlobContainerClient(container);
                    await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                    var blobClient = containerClient.GetBlobClient(path);

                    if (await blobClient.ExistsAsync())
                    {
                        return new Error("You've already uploaded this! Please delete it first if you want to replace it.");
                    }

                    await blobClient.UploadAsync(blob, new BlobHttpHeaders { ContentType = contentType }, metadata);
                }

                return null;
            }
            catch (RequestFailedException rfe)
            {
                return new Error($"The request to azure storage failed: {rfe.Message}");
            }
            catch (Exception e)
            {
                return new Error($"An unexpected error happened: {e.Message}");
            }
        }

        private (string Container, string Path) Parse(string path)
        {
            var segments = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            return (
                segments.First().ToLower(),
                string.Join('/', segments.Skip(1).Take(segments.Count() - 1))
            );
        }
    }
}