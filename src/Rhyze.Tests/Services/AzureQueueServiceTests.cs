using Azure.Storage.Queues;
using Moq;
using Rhyze.Core.Messages;
using Rhyze.Services;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.Services
{
    [Trait(nameof(Services), nameof(AzureQueueService))]
    public class AzureQueueServiceTests
    {
        private readonly AzureQueueService _service;
        private readonly Mock<QueueClient> _client = new Mock<QueueClient>();

        public AzureQueueServiceTests()
        {
            _service = new AzureQueueService((_, queueName) =>
            {
                _client.Setup(c => c.Name).Returns(queueName);

                return _client.Object;
            });
        }

        [Fact]
        public async Task EnqueueAlbumDeletionAsync_Ensures_The_Queue_Exists()
        {
            var expectedQueueName = AzureQueueService.AlbumDeletionQueue;
            var msg = new DeleteAlbumMessage { };

            await _service.EnqueueAlbumDeletionAsync(msg);

            Assert.Equal(expectedQueueName, _client.Object.Name);
            _client.Verify(c => c.CreateIfNotExistsAsync(null, default), Times.Once());
        }

        [Fact]
        public async Task EnqueueAlbumDeletionAsync_Enqueues_A_Base_64_Json_Message()
        {
            var albumName = "Cafe de Touhou 8";
            var ownerId = Guid.Empty;
            var msg = new DeleteAlbumMessage { AlbumName = albumName, OwnerId = ownerId };
            var json = $"{{\"AlbumName\":\"{albumName}\",\"OwnerId\":\"{ownerId}\"}}";

            var expectedMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

            await _service.EnqueueAlbumDeletionAsync(msg);

            _client.Verify(c => c.SendMessageAsync(expectedMessage), Times.Once());
        }

        [Fact]
        public async Task EnqueueTrackUploadedAsync_Ensures_The_Queue_Exists()
        {
            var expectedQueueName = AzureQueueService.TrackUploadedQueue;
            var msg = new TrackUploadedMessage { Name = "" };

            await _service.EnqueueTrackUploadedAsync(msg);

            Assert.Equal(expectedQueueName, _client.Object.Name);
            _client.Verify(c => c.CreateIfNotExistsAsync(null, default), Times.Once());
        }

        [Fact]
        public async Task EnqueueTrackUploadedAsync_Enqueues_A_Base_64_String_Message()
        {
            var name = "01/02/03/0102030405";
            var msg = new TrackUploadedMessage { Name = name };

            var expectedMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(name));

            await _service.EnqueueTrackUploadedAsync(msg);

            _client.Verify(c => c.SendMessageAsync(expectedMessage), Times.Once());
        }
    }
}
