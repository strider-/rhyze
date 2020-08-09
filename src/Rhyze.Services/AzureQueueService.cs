using Azure.Storage.Queues;
using Rhyze.Core.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Rhyze.Services
{
    public class AzureQueueService : IQueueService
    {
        public const string AlbumDeletionQueue = "album-delete";

        private readonly string _connStr;
        private readonly Func<string, string, QueueClient> _clientFactory;

        public AzureQueueService(string connectionString)
        {
            _connStr = connectionString;
            _clientFactory = (c, q) => new QueueClient(c, q);
        }

        /// <summary>
        /// This contructor is primarily for testing and is not meant to be used by your code.
        /// </summary>
        public AzureQueueService(Func<string, string, QueueClient> factory)
        {
            _clientFactory = factory;
        }

        public Task EnqueueAlbumDeletionAsync(string albumName) => EnqueueAsync(AlbumDeletionQueue, albumName);

        private async Task EnqueueAsync(string queueName, string message)
        {
            var client = _clientFactory(_connStr, queueName);

            await client.CreateIfNotExistsAsync();

            var payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(message));

            await client.SendMessageAsync(payload);
        }
    }
}