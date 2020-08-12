using Moq;
using Rhyze.API.Commands;
using Rhyze.Core.Models;
using Rhyze.Data;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Commands
{
    [Trait("API.Commands", nameof(UploadTracksCommandHandler))]
    public class UpdateAlbumMetadataCommandHandlerTests
    {
        private readonly UpdateAlbumMetadataCommandHandler _handler;
        private readonly Mock<IDatabase> _db = new Mock<IDatabase>();

        public UpdateAlbumMetadataCommandHandlerTests()
        {
            _handler = new UpdateAlbumMetadataCommandHandler(_db.Object);
        }

        [Fact]
        public async Task Handle_Returns_The_Updated_Metadata()
        {
            var request = new UpdateAlbumMetadataCommand
            {
                Album = "Blackmagik Blazing",
                AlbumArtist = "かめりあ",
                DiscCount = 1,
                Id = new AlbumId("かめりあ", "Blackmagic Blazing"),
                OwnerId = Guid.NewGuid(),
                TrackCount = 14,
                Year = 2019
            };

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
        }
    }
}
