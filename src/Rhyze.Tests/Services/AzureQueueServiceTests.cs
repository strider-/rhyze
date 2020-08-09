using Azure.Storage.Queues;
using Moq;
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

            await _service.EnqueueAlbumDeletionAsync("");

            Assert.Equal(expectedQueueName, _client.Object.Name);
            _client.Verify(c => c.CreateIfNotExistsAsync(null, default), Times.Once());
        }

        [Fact]
        public async Task EnqueueAlbumDeletionAsync_Enqueues_A_Base_64_Message()
        {
            var albumName = "Cafe de Touhou 8";
            var expectedMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(albumName));

            await _service.EnqueueAlbumDeletionAsync(albumName);

            _client.Verify(c => c.SendMessageAsync(expectedMessage), Times.Once());
        }
    }
}
