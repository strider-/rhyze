using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Rhyze.Core.Messages;
using Rhyze.Data;
using Rhyze.Data.Commands;
using Rhyze.Data.Queries;
using Rhyze.Services;
using System.IO;
using System.Threading.Tasks;

namespace Rhyze.Functions
{
    public class AlbumFunctions
    {
        private readonly IDatabase _db;

        public AlbumFunctions(IDatabase db) => _db = db;

        [FunctionName("DequeueAlbumDeletion")]
        public async Task DequeueAlbumDeletionAsync(
            [QueueTrigger(AzureQueueService.AlbumDeletionQueue)] DeleteAlbumMessage msg,
            [Blob("audio", FileAccess.Write)] CloudBlobContainer audioContainer,
            [Blob("image", FileAccess.Write)] CloudBlobContainer imageContainer,
            ILogger log
        )
        {
            log.LogInformation($"Recieved album to delete: {msg.AlbumName}");

            var tracks = await _db.ExecuteAsync(new GetAlbumByNameQuery(msg.OwnerId, msg.AlbumName));

            foreach (var track in tracks)
            {
                var name = track.GetBlobName();
                log.LogTrace($"Deleting Track '{track.Title}' (name: {name})");

                await audioContainer.GetBlockBlobReference(name).DeleteIfExistsAsync();
                log.LogTrace("\tAudio blob deleted.");

                if (track.ImageUrl != null)
                {
                    await imageContainer.GetBlockBlobReference(name).DeleteIfExistsAsync();
                    log.LogTrace("\tImage blob deleted.");
                }
                else
                {
                    log.LogTrace("\tNo image blob for this track.");
                }
            }

            await _db.ExecuteAsync(new HardDeleteAlbumCommand(tracks));

            log.LogInformation($"{msg.AlbumName} has been deleted.");
        }
    }
}