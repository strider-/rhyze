using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Rhyze.Core.Interfaces;
using Rhyze.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Rhyze.Functions
{
    public class TrackFunctions
    {
        [FunctionName("OnTrackUploaded")]
        public async Task OnTrackUploadedAsync(
            [BlobTrigger("audio/{name}")] Stream audio, BlobProperties properties, IDictionary<string, string> metadata, Uri uri, ILogger log,
            [Blob("image/{name}", FileAccess.Write)] CloudBlockBlob image)
        {
            // Do this up with proper DI you idiot
            ITagReader reader = new TagLibReader();
            var ownerId = Guid.Parse(metadata["ownerid"]);
            string imageUri = null;

            var tag = await reader.ReadTagAsync(audio, properties.ContentType);

            if (tag.HasArtwork)
            {
                if (image.Container.Properties.PublicAccess != BlobContainerPublicAccessType.Blob)
                {
                    await image.Container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }
                image.UploadFromStream(tag.Artwork);
                image.Properties.ContentType = tag.ArtworkMimeType;
                await image.SetPropertiesAsync();
                imageUri = image.Uri.AbsoluteUri;
            }

            var track = tag.ToTrack(ownerId, uri.AbsoluteUri, imageUri);

            log.LogInformation($"Processed Track:\n\t{JsonConvert.SerializeObject(track, Formatting.Indented)}");
        }
    }
}