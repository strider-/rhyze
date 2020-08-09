using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Rhyze.Services;
using System.Threading.Tasks;

namespace Rhyze.Functions
{
    public class AlbumFunctions
    {
        [FunctionName("DequeueAlbumDeletion")]
        public async Task DequeueAlbumDeletionAsync(
            [QueueTrigger(AzureQueueService.AlbumDeletionQueue)] string albumName,
            ILogger log
        )
        {
            log.LogInformation($"Recieved album to delete: {albumName}");
        }
    }
}
