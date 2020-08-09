using Moq;
using Rhyze.API.Commands;
using Rhyze.Core.Interfaces;
using Rhyze.Data;
using Rhyze.Data.Commands;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Commands
{
    [Trait("API.Commands", nameof(DeleteAlbumCommandHandler))]
    public class DeleteAlbumCommandHandlerTests
    {
        private readonly DeleteAlbumCommandHandler _hander;
        private readonly Mock<IQueueService> _service = new Mock<IQueueService>();
        private readonly Mock<IDatabase> _db = new Mock<IDatabase>();

        public DeleteAlbumCommandHandlerTests()
        {
            _hander = new DeleteAlbumCommandHandler(_service.Object, _db.Object);
        }

        [Fact]
        public async Task Handle_Soft_Deletes_The_Album()
        {
            var expectedName = "かめりあ - Xroniàl Xéro";
            var request = new DeleteAlbumCommand { Name = expectedName };

            await _hander.Handle(request, default);

            _db.Verify(db => db.ExecuteAsync(It.Is<SoftDeleteAlbumCommand>(c =>
                c.AlbumName == expectedName
            )), Times.Once());
        }

        [Fact]
        public async Task Handle_Enqueues_Hard_Deletion()
        {
            var expectedName = "EXIT TUNES PRESENTS Vocaloconnection feat. 初音ミク";
            var id = Guid.NewGuid();
            var request = new DeleteAlbumCommand { Name = expectedName, OwnerId = id };

            await _hander.Handle(request, default);

            _service.Verify(s => s.EnqueueAlbumDeletionAsync(id, expectedName), Times.Once());
        }
    }
}
