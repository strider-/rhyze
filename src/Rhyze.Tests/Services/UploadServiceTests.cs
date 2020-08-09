using Moq;
using Rhyze.Core.Interfaces;
using Rhyze.Core.Messages;
using Rhyze.Core.Models;
using Rhyze.Services;
using Rhyze.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.Services
{
    [Trait(nameof(Services), nameof(UploadService))]
    public class UploadServiceTests
    {
        private readonly UploadService _service;
        private readonly Mock<IBlobStore> _store = new Mock<IBlobStore>();
        private readonly Mock<IQueueService> _queueService = new Mock<IQueueService>();
        private readonly HMAC _hasher = new FakeHashAlgorithm();
        private readonly Stream _stream = new MemoryStream();

        public UploadServiceTests()
        {
            _service = new UploadService(_store.Object, _queueService.Object, _hasher);
        }

        [Fact]
        public async Task UploadTrackAsync_Returns_An_Error_With_An_Invalid_ContentType()
        {
            var ct = "invalid/type";

            var error = await _service.UploadTrackAsync(Guid.NewGuid(), ct, _stream);

            Assert.NotNull(error);
            Assert.Equal($"Content type of '{ct}' is not supported.", error.ToString());
            _store.Verify(s => s.UploadAsync(It.IsAny<string>(), _stream, ct, It.IsAny<IDictionary<string, string>>()), Times.Never());
        }

        [Fact]
        public async Task UploadTrackAsync_Returns_An_Error_With_An_Invalid_Owner()
        {
            var ct = "audio/x-flac";

            var error = await _service.UploadTrackAsync(Guid.Empty, ct, _stream);

            Assert.NotNull(error);
            Assert.Equal("The owner id cannot be an empty guid.", error.ToString());
            _store.Verify(s => s.UploadAsync(It.IsAny<string>(), _stream, ct, It.IsAny<IDictionary<string, string>>()), Times.Never());
        }

        [Fact]
        public async Task UploadTrackAsync_Returns_Errors_From_The_Store()
        {
            var ct = "audio/mpeg";
            var expectedError = new Error("This will fail.");

            _store.Setup(s => s.UploadAsync(It.IsAny<string>(), _stream, ct, It.IsAny<IDictionary<string, string>>()))
                  .ReturnsAsync(expectedError);

            var error = await _service.UploadTrackAsync(Guid.NewGuid(), ct, _stream);

            Assert.NotNull(expectedError);
            Assert.Equal(expectedError.ToString(), error.ToString());
        }

        [Fact]
        public async Task UploadTrackAsync_Enqueues_A_TrackUploadedMessage_On_Successful_Upload()
        {
            var id = Guid.NewGuid();
            var ct = "audio/mpeg";
            var name = "010/203/040/0102030405060708090a";
            var expectedBlobPath = $"audio/{name}";
            _store.Setup(s => s.UploadAsync(expectedBlobPath, _stream, ct, It.IsAny<IDictionary<string, string>>()))
                  .ReturnsAsync((Error)null);

            var error = await _service.UploadTrackAsync(id, ct, _stream);

            Assert.Null(error);
            _queueService.Verify(q => q.EnqueueTrackUploadedAsync(It.Is<TrackUploadedMessage>(
                m => m.Name == name)
            ), Times.Once());
        }

        [Fact]
        public async Task UploadTrackAsync_Is_Successful()
        {
            var id = Guid.NewGuid();
            var ct = "audio/flac";
            var name = "010/203/040/0102030405060708090a";
            var expectedBlobPath = $"audio/{name}";
            Expression<Func<IDictionary<string, string>, bool>> expectedMetadata = d => d["ownerId"] == id.ToString();
            _store.Setup(s => s.UploadAsync(expectedBlobPath, _stream, ct, It.IsAny<IDictionary<string, string>>()))
                  .ReturnsAsync((Error)null);

            var error = await _service.UploadTrackAsync(id, ct, _stream);

            Assert.Null(error);
            _store.Verify(s => s.UploadAsync(expectedBlobPath, _stream, ct, It.Is(expectedMetadata)), Times.Once());
        }
    }
}