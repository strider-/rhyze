using Moq;
using Rhyze.API.Queries;
using Rhyze.Core.Models;
using Rhyze.Data;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Queries
{
    [Trait("API.Queries", nameof(GetAlbumMetadataQueryHandler))]
    public class GetAlbumMetadataQueryHandlerTests
    {
        private readonly Guid _ownerId = Guid.NewGuid();
        private readonly GetAlbumMetadataQueryHandler _handler;
        private readonly Mock<IDatabase> _db = new Mock<IDatabase>();

        public GetAlbumMetadataQueryHandlerTests()
        {
            _handler = new GetAlbumMetadataQueryHandler(_db.Object);
        }

        [Fact]
        public async Task Handle_Executes_The_Expected_Database_Query()
        {
            var albumId = new AlbumId("コトノハルカナ", "BlackBlackCandy");
            var request = new GetAlbumMetadataQuery { AlbumId = albumId, OwnerId = _ownerId };

            await _handler.Handle(request, default);

            _db.Verify(db => db.ExecuteAsync(It.Is<Rhyze.Data.Queries.GetAlbumMetadataQuery>(q =>
                q.AlbumId.Name == albumId.Name &&
                q.AlbumId.AlbumArtist == albumId.AlbumArtist &&
                q.OwnerId == _ownerId
            )), Times.Once());
        }
    }
}
