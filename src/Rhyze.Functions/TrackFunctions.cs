using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Rhyze.Core.Interfaces;
using Rhyze.Core.Models;
using Rhyze.Data;
using Rhyze.Data.Commands;
using Rhyze.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Rhyze.Functions
{
    public class TrackFunctions
    {
        private readonly ITagReader _tagReader;
        private readonly IDatabase _db;

        public TrackFunctions(ITagReader tagReader, IDatabase db)
        {
            _tagReader = tagReader;
            _db = db;
        }

        [FunctionName("OnTrackUploaded")]
        public async Task OnTrackUploadedAsync(
            [QueueTrigger(AzureQueueService.TrackUploadedQueue)] string name,
            [Blob("audio/{queueTrigger}")] ICloudBlob audioBlob,
            [Blob("image/{queueTrigger}", FileAccess.Write)] CloudBlobContainer imageContainer,
            ILogger log)
        {
            CloudBlockBlob imageBlob = null;

            try
            {
                await audioBlob.FetchAttributesAsync();

                var ownerId = Guid.Parse(audioBlob.Metadata["ownerid"]);
                var tag = await _tagReader.ReadTagAsync(await audioBlob.OpenReadAsync(), audioBlob.Properties.ContentType);
                imageBlob = await UploadArtworkAsync(ownerId, name, imageContainer, tag);

                var track = tag.ToTrack(ownerId, audioBlob.Uri.AbsoluteUri, imageBlob?.Uri.AbsoluteUri);

                await _db.ExecuteAsync(new CreateTrackCommand(track));

                log.LogInformation($"Processed New Track Id {track.Id}.");
                log.LogTrace($"Track Detail:\n\t{JsonConvert.SerializeObject(track, Formatting.Indented)}");
            }
            catch (Exception e)
            {
                log.LogCritical($"Exception Thrown: {e}");
                if (imageBlob != null)
                {
                    await imageBlob.DeleteAsync();
                }
                await audioBlob.DeleteAsync();
            }
        }

        private async Task<CloudBlockBlob> UploadArtworkAsync(Guid ownerId, string imageName, CloudBlobContainer container, AudioTag tag)
        {
            if (tag.HasArtwork)
            {
                await container.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Blob, new BlobRequestOptions(), new OperationContext());
                var image = container.GetBlockBlobReference(imageName);

                image.UploadFromStream(tag.Artwork);
                image.Properties.ContentType = tag.ArtworkMimeType;
                image.Metadata["ownerid"] = ownerId.ToString();

                await image.SetMetadataAsync();
                await image.SetPropertiesAsync();

                return image;
            }

            return null;
        }
    }
}