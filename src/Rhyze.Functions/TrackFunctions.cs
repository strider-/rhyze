using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Rhyze.Functions
{
    public class TrackFunctions
    {
        [FunctionName("OnTrackUploaded")]
        public void OnTrackUploaded([BlobTrigger("audio/{name}")] Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}