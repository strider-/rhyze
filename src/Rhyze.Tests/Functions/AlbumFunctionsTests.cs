using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using Moq;
using Rhyze.Core.Messages;
using Rhyze.Core.Models;
using Rhyze.Data;
using Rhyze.Data.Commands;
using Rhyze.Data.Queries;
using Rhyze.Functions;
using Rhyze.Tests.Fixtures;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.Functions
{
    [Trait(nameof(Functions), nameof(AlbumFunctions))]
    public class AlbumFunctionsTests
    {
        private readonly Guid _ownerId = Guid.NewGuid();
        private readonly AlbumFunctions _func;
        private readonly Mock<IDatabase> _db = new Mock<IDatabase>();
        private readonly Mock<CloudBlobContainer> _audioContainer = new Mock<CloudBlobContainer>(new Uri("http://store/audio"));
        private readonly Mock<CloudBlockBlob> _audioBlob = new Mock<CloudBlockBlob>(new Uri("http://store/audio/file"));
        private readonly Mock<CloudBlobContainer> _imageContainer = new Mock<CloudBlobContainer>(new Uri("http://store/image"));
        private readonly Mock<CloudBlockBlob> _imageBlob = new Mock<CloudBlockBlob>(new Uri("http://store/image/file"));

        public AlbumFunctionsTests()
        {
            _func = new AlbumFunctions(_db.Object);

            _audioContainer.Setup(c => c.GetBlockBlobReference(It.IsAny<string>()))
                           .Returns(_audioBlob.Object);

            _imageContainer.Setup(c => c.GetBlockBlobReference(It.IsAny<string>()))
                           .Returns(_imageBlob.Object);
        }

        [Fact]
        public async Task DequeueAlbumDeletionAsync_Fetches_Album_Tracks()
        {
            var albumId = new AlbumId("QUxpQ0UnUyBFTU9UaU9OCUJsb3Nzb20gQ0VOU09SRUQhISBFLlAu");
            var msg = new DeleteAlbumMessage
            {
                AlbumIdValue = albumId.Value,
                OwnerId = _ownerId
            };

            await _func.DequeueAlbumDeletionAsync(msg, _audioContainer.Object, _imageContainer.Object, Mock.Of<ILogger>());

            _db.Verify(db => db.ExecuteAsync(It.Is<GetAlbumByIdQuery>(q => 
                q.AlbumId.Value == msg.AlbumIdValue && 
                q.OwnerId == _ownerId)
            ), Times.Once());
        }

        [Fact]
        public async Task DequeueAlbumDeletionAsync_Deletes_Existing_Blobs()
        {
            var msg = new DeleteAlbumMessage { AlbumIdValue = "QUxpQ0UnUyBFTU9UaU9OCUJsb3Nzb20gQ0VOU09SRUQhISBFLlAu" };
            var tracks = MultipleTracks();
            _db.Setup(db => db.ExecuteAsync(It.IsAny<GetAlbumByIdQuery>()))
               .ReturnsAsync(tracks);

            await _func.DequeueAlbumDeletionAsync(msg, _audioContainer.Object, _imageContainer.Object, Mock.Of<ILogger>());

            _audioBlob.Verify(b => b.DeleteIfExistsAsync(), Times.Exactly(2));
            _imageBlob.Verify(b => b.DeleteIfExistsAsync(), Times.Once());
        }

        [Fact]
        public async Task DequeueAlbumDeletionAsync_Hard_Deletes_The_Album()
        {
            var msg = new DeleteAlbumMessage { AlbumIdValue = "QUxpQ0UnUyBFTU9UaU9OCUJsb3Nzb20gQ0VOU09SRUQhISBFLlAu" };
            var tracks = SingleTrack();
            _db.Setup(db => db.ExecuteAsync(It.IsAny<GetAlbumByIdQuery>()))
               .ReturnsAsync(tracks);

            await _func.DequeueAlbumDeletionAsync(msg, _audioContainer.Object, _imageContainer.Object, Mock.Of<ILogger>());

            _db.Verify(db => db.ExecuteAsync(It.Is<HardDeleteAlbumCommand>(q => q.ToBeDeleted == tracks)), Times.Once());
        }

        [Fact]
        public async Task DequeueAlbumDeletionAsync_Writes_To_The_Log()
        {
            var albumId = new AlbumId("D.watt", "Opium and Purple Haze EP");
            var msg = new DeleteAlbumMessage
            {
                AlbumIdValue = albumId.Value,
                OwnerId = _ownerId
            };
            var log = new MockLogger();
            _db.Setup(db => db.ExecuteAsync(It.IsAny<GetAlbumByIdQuery>()))
               .ReturnsAsync(SingleTrack());

            await _func.DequeueAlbumDeletionAsync(msg, _audioContainer.Object, _imageContainer.Object, log.Object);

            log.Verify(LogLevel.Information, $"Recieved album to delete: {albumId.Name}");
            log.Verify(LogLevel.Trace, $"Deleting Track '01' (name: file)");
            log.Verify(LogLevel.Trace, "\tAudio blob deleted.");
            log.Verify(LogLevel.Trace, "\tImage blob deleted.");
            log.Verify(LogLevel.Information, $"{albumId.Name} has been deleted.");
        }

        private Track[] SingleTrack()
        {
            return new[]
            {
                new Track{ Title = "01", AudioUrl = _audioBlob.Object.Uri.AbsoluteUri, ImageUrl = _imageBlob.Object.Uri.AbsoluteUri  }
            };
        }

        private Track[] MultipleTracks()
        {
            return SingleTrack().Concat(new[] { new Track { Title = "02", AudioUrl = _audioBlob.Object.Uri.AbsoluteUri } }).ToArray();
        }
    }
}