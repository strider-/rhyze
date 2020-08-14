using Moq;
using Rhyze.API.Requests;
using Rhyze.Core.Models;
using Rhyze.Data;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Requests
{
    [Trait("API.Queries", nameof(GetAlbumMetadataRequestHandler))]
    public class GetAlbumMetadataRequestHandlerTests
    {
        private readonly Guid _ownerId = Guid.NewGuid();
        private readonly GetAlbumMetadataRequestHandler _handler;
        private readonly Mock<IDatabase> _db = new Mock<IDatabase>();

        public GetAlbumMetadataRequestHandlerTests()
        {
            _handler = new GetAlbumMetadataRequestHandler(_db.Object);
        }

        [Fact]
        public async Task Handle_Executes_The_Expected_Database_Query()
        {
            var albumId = new AlbumId("コトノハルカナ", "BlackBlackCandy");
            var request = new GetAlbumMetadataRequest { AlbumId = albumId, OwnerId = _ownerId };

            await _handler.Handle(request, default);

            _db.Verify(db => db.ExecuteAsync(It.Is<Rhyze.Data.Queries.GetAlbumMetadataQuery>(q =>
                q.AlbumId.Name == albumId.Name &&
                q.AlbumId.AlbumArtist == albumId.AlbumArtist &&
                q.OwnerId == _ownerId
            )), Times.Once());
        }
    }
}
