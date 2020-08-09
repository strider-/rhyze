using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using Moq;
using Rhyze.Core.Interfaces;
using Rhyze.Core.Models;
using Rhyze.Data;
using Rhyze.Data.Commands;
using Rhyze.Functions;
using Rhyze.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.Functions
{
    [Trait(nameof(Functions), nameof(TrackFunctions))]
    public class TrackFunctionsTests
    {
        private readonly TrackFunctions _func;
        private readonly Mock<ITagReader> _reader = new Mock<ITagReader>();
        private readonly Mock<IDatabase> _db = new Mock<IDatabase>();
        private readonly Mock<ICloudBlob> _blob = new Mock<ICloudBlob>();
        private readonly Mock<CloudBlockBlob> _imageBlob = new Mock<CloudBlockBlob>(new Uri("http://store/container/img"));
        private readonly Mock<CloudBlobContainer> _container = new Mock<CloudBlobContainer>(new Uri("http://store/container"));
        private readonly Guid _ownerId = Guid.NewGuid();
        private readonly MemoryStream _stream = new MemoryStream();

        public TrackFunctionsTests()
        {
            _func = new TrackFunctions(_reader.Object, _db.Object);

            _reader.Setup(r => r.ReadTagAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                   .ReturnsAsync(new AudioTag());

            _blob.Setup(b => b.Metadata).Returns(new Dictionary<string, string>
            {
                { "ownerid", _ownerId.ToString() }
            });
            _blob.Setup(b => b.Uri).Returns(new Uri("http://store/container/file"));
            _blob.Setup(b => b.OpenReadAsync()).ReturnsAsync(_stream);
            _blob.Setup(b => b.Properties).Returns(new BlobProperties
            {
                ContentType = "audio/flac"
            });

            _container.Setup(c => c.GetBlockBlobReference(It.IsAny<string>()))
                      .Returns(_imageBlob.Object);
        }

        [Fact]
        public async Task OnTrackUploadedAsync_Reads_Metadata()
        {
            await _func.OnTrackUploadedAsync("", _blob.Object, _container.Object, Mock.Of<ILogger>());

            _reader.Verify(r => r.ReadTagAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task OnTrackUploadedAsync_Persists_The_New_Track()
        {
            await _func.OnTrackUploadedAsync("", _blob.Object, _container.Object, Mock.Of<ILogger>());

            _db.Verify(db => db.ExecuteAsync(It.IsAny<CreateTrackCommand>()), Times.Once());
        }

        [Fact]
        public async Task OnTrackUploadedAsync_Stores_Track_Artwork_If_Present()
        {
            var name = "01/02/03/lol";
            var ct = "image/jpeg";
            _reader.Setup(r => r.ReadTagAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                   .ReturnsAsync(new AudioTag { Artwork = _stream, ArtworkMimeType = ct });

            await _func.OnTrackUploadedAsync(name, _blob.Object, _container.Object, Mock.Of<ILogger>());

            Assert.Equal(_ownerId.ToString(), _imageBlob.Object.Metadata["ownerid"]);
            Assert.Equal(ct, _imageBlob.Object.Properties.ContentType);
            _container.Verify(c => c.GetBlockBlobReference(name), Times.Once());
            _imageBlob.Verify(b => b.UploadFromStream(_stream, null, null, null), Times.Once());
            _imageBlob.Verify(b => b.SetPropertiesAsync(), Times.Once());
            _imageBlob.Verify(b => b.SetMetadataAsync(), Times.Once());
            _container.Verify(c =>
                c.CreateIfNotExistsAsync(
                    BlobContainerPublicAccessType.Blob,
                    It.IsAny<BlobRequestOptions>(),
                    It.IsAny<OperationContext>()
                ), Times.Once()
            );
        }

        [Fact]
        public async Task OnTrackUploadedAsync_Logs_Track_Information()
        {
            var log = new MockLogger();

            await _func.OnTrackUploadedAsync("", _blob.Object, _container.Object, log.Object);

            log.Verify(Microsoft.Extensions.Logging.LogLevel.Information, "Processed New Track Id");
            log.Verify(Microsoft.Extensions.Logging.LogLevel.Trace, "Track Detail:");
        }

        [Fact]
        public async Task OnTrackUploadedAsync_Logs_Exceptions()
        {
            var log = new MockLogger();
            var e = new Exception("oh no!");
            _reader.Setup(r => r.ReadTagAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                   .ThrowsAsync(e);

            await _func.OnTrackUploadedAsync("", _blob.Object, _container.Object, log.Object);

            log.Verify(Microsoft.Extensions.Logging.LogLevel.Critical, $"Exception Thrown: {e}");
        }

        [Fact]
        public async Task OnTrackUploadedAsync_Deletes_Audio_And_Image_Blobs_On_Exceptions()
        {
            var log = new MockLogger();
            var e = new Exception("oh no!");
            _reader.Setup(r => r.ReadTagAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                   .ReturnsAsync(new AudioTag { Artwork = _stream, ArtworkMimeType = "image/png" });
            _db.Setup(db => db.ExecuteAsync(It.IsAny<CreateTrackCommand>()))
               .ThrowsAsync(e);

            await _func.OnTrackUploadedAsync("", _blob.Object, _container.Object, log.Object);

            _blob.Verify(b => b.DeleteAsync(), Times.Once());
            _imageBlob.Verify(b => b.DeleteAsync(), Times.Once());
        }
    }
}