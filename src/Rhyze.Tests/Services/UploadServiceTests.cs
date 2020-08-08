using Moq;
using Rhyze.Core.Interfaces;
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
        private readonly HashAlgorithm _hasher = new FakeHashAlgorithm();

        public UploadServiceTests()
        {
            _service = new UploadService(_store.Object, _hasher);
        }

        [Fact]
        public async Task UploadTrackAsync_Returns_An_Error_With_An_Invalid_ContentType()
        {
            var stream = new MemoryStream();
            var ct = "invalid/type";

            var error = await _service.UploadTrackAsync(Guid.NewGuid(), ct, stream);

            Assert.NotNull(error);
            Assert.Equal($"Content type of '{ct}' is not supported.", error.ToString());
            _store.Verify(s => s.UploadAsync(It.IsAny<string>(), stream, ct, It.IsAny<IDictionary<string, string>>()), Times.Never());
        }

        [Fact]
        public async Task UploadTrackAsync_Returns_Errors_From_The_Store()
        {
            var stream = new MemoryStream();
            var ct = "audio/mpeg";
            var expectedError = new Error("This will fail.");

            _store.Setup(s => s.UploadAsync(It.IsAny<string>(), stream, ct, It.IsAny<IDictionary<string, string>>()))
                  .ReturnsAsync(expectedError);

            var error = await _service.UploadTrackAsync(Guid.NewGuid(), ct, stream);

            Assert.NotNull(expectedError);
            Assert.Equal(expectedError.ToString(), error.ToString());
        }

        [Fact]
        public async Task UploadTrackAsync_Is_Successful()
        {
            var id = Guid.NewGuid();
            var stream = new MemoryStream();
            var ct = "audio/flac";
            var expectedBlobPath = "audio/010/203/040/0102030405060708090a";
            Expression<Func<IDictionary<string, string>, bool>> expectedMetadata = d => d["ownerId"] == id.ToString();
            _store.Setup(s => s.UploadAsync(It.IsAny<string>(), stream, ct, It.IsAny<IDictionary<string, string>>()))
                  .ReturnsAsync((Error)null);

            var error = await _service.UploadTrackAsync(id, ct, stream);

            Assert.Null(error);
            _store.Verify(s => s.UploadAsync(expectedBlobPath, stream, ct, It.Is(expectedMetadata)), Times.Once());
        }
    }
}