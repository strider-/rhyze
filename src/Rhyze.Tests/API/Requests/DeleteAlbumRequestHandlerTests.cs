using Moq;
using Rhyze.API.Requests;
using Rhyze.Core.Interfaces;
using Rhyze.Core.Messages;
using Rhyze.Core.Models;
using Rhyze.Data;
using Rhyze.Data.Commands;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Requests
{
    [Trait("API.Requests", nameof(DeleteAlbumRequestHandler))]
    public class DeleteAlbumRequestHandlerTests
    {
        private readonly DeleteAlbumRequestHandler _hander;
        private readonly Mock<IQueueService> _service = new Mock<IQueueService>();
        private readonly Mock<IDatabase> _db = new Mock<IDatabase>();

        public DeleteAlbumRequestHandlerTests()
        {
            _hander = new DeleteAlbumRequestHandler(_service.Object, _db.Object);
        }

        [Fact]
        public async Task Handle_Soft_Deletes_The_Album()
        {
            var albumId = new AlbumId("かめりあ", "かめりあ - Xroniàl Xéro");
            var request = new DeleteAlbumRequest { Id = albumId };

            await _hander.Handle(request, default);

            _db.Verify(db => db.ExecuteAsync(It.Is<SoftDeleteAlbumCommand>(c =>
                c.AlbumId.Name == albumId.Name && c.AlbumId.AlbumArtist == albumId.AlbumArtist
            )), Times.Once());
        }

        [Fact]
        public async Task Handle_Enqueues_Hard_Deletion()
        {
            var albumId = new AlbumId("EXIT TUNES", "EXIT TUNES PRESENTS Vocaloconnection feat. 初音ミク");
            var id = Guid.NewGuid();
            var request = new DeleteAlbumRequest { Id = albumId, OwnerId = id };

            await _hander.Handle(request, default);

            _service.Verify(s => s.EnqueueAlbumDeletionAsync(It.Is<DeleteAlbumMessage>(m =>
                m.AlbumIdValue == albumId.Value && m.OwnerId == id
            )), Times.Once());
        }
    }
}
