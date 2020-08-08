using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Moq;
using Rhyze.Services;
using Rhyze.Tests.Fixtures;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.Services
{
    [Trait(nameof(Services), nameof(AzureBlobStore))]
    public class AzureBlobStoreTests
    {
        const string BlobPath = "root/dir/filename";

        private readonly AzureBlobStore _store;
        private readonly Mock<BlobServiceClient> _client = new Mock<BlobServiceClient>(MockBehavior.Strict);
        private readonly Mock<BlobContainerClient> _containerClient = new Mock<BlobContainerClient>(MockBehavior.Strict);
        private readonly Mock<BlobClient> _blobClient = new Mock<BlobClient>(MockBehavior.Strict);
        private readonly Stream _stream = new MemoryStream();

        public AzureBlobStoreTests()
        {
            _store = new AzureBlobStore(_client.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task UploadAsync_Returns_An_Error_When_The_ContentType_Is_Missing(string ct)
        {
            var error = await _store.UploadAsync(BlobPath, new MemoryStream(), ct);

            Assert.NotNull(error);
            Assert.Equal("Refusing to upload data without a content type.", error.ToString());
        }

        [Fact]
        public async Task UploadAsync_Returns_An_Error_If_The_Blob_Already_Exists()
        {
            SetupAzureMocks(blobExists: true);

            var error = await _store.UploadAsync(BlobPath, new MemoryStream(), "dummy/type", null);

            Assert.NotNull(error);
            Assert.Equal("You've already uploaded this! Please delete it first if you want to replace it.", error.ToString());
        }

        [Fact]
        public async Task UploadAsync_Returns_An_Error_Upload_Failed()
        {
            var ct = "dummy/type";
            SetupAzureMocks(
                blobExists: false, 
                uploadCausesException: true, 
                stream: _stream, 
                contentType: ct
            );

            var error = await _store.UploadAsync(BlobPath, _stream, ct, null);

            Assert.NotNull(error);
            Assert.Equal("The request to azure storage failed: thrown", error.ToString());
        }

        [Fact]
        public async Task UploadAsync_Handles_Unexpected_Errors()
        {
            var ct = "dummy/type";
            SetupAzureMocks(blobExists: false);

            var error = await _store.UploadAsync(null, _stream, ct, null);

            Assert.NotNull(error);
            Assert.Equal("An unexpected error happened: Object reference not set to an instance of an object.", error.ToString());
        }

        [Fact]
        public async Task UploadAsync_Succeeds()
        {
            var ct = "dummy/type";
            var metadata = new Dictionary<string, string> { { "ownerId", "test" } };
            SetupAzureMocks(
                blobExists: false,
                stream: _stream,
                contentType: ct,
                metadata: metadata
            );

            var error = await _store.UploadAsync(BlobPath, _stream, ct, metadata);

            Assert.Null(error);
            _blobClient.Verify(b => b.UploadAsync(
                _stream, It.Is<BlobHttpHeaders>(h => h.ContentType == ct), metadata, null, null, null, default, default
            ), Times.Once());
        }

        private void SetupAzureMocks(bool blobExists = false, bool uploadCausesException = false, Stream stream = null, string contentType = null, IDictionary<string, string> metadata = null)
        {
            var container = BlobPath.Substring(0, BlobPath.IndexOf('/'));
            var path = BlobPath.Substring(BlobPath.IndexOf('/') + 1);

            _client.Setup(c => c.GetBlobContainerClient(container))
                    .Returns(_containerClient.Object);

            _containerClient.Setup(c => c.CreateIfNotExistsAsync(PublicAccessType.Blob, null, null, default))
                   .ReturnsAsync(Mock.Of<Response<BlobContainerInfo>>());

            _containerClient.Setup(c => c.GetBlobClient(path))
                   .Returns(_blobClient.Object);

            _blobClient.Setup(b => b.ExistsAsync(default)).ReturnsAsync(new FakeAzureResponse<bool>(blobExists));

            var uploadSetup = _blobClient.Setup(
                b => b.UploadAsync(
                    stream, It.Is<BlobHttpHeaders>(h => h.ContentType == contentType), metadata, null, null, null, default, default
                )
            );

            if (uploadCausesException)
            {
                uploadSetup.ThrowsAsync(new RequestFailedException(0, "thrown"));
            }
            else
            {
                uploadSetup.ReturnsAsync(Mock.Of<Response<BlobContentInfo>>());
            }
        }
    }
}
