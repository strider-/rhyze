using Moq;
using Rhyze.API.Requests;
using Rhyze.Core.Models;
using Rhyze.Data;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Requests
{
    [Trait("API.Commands", nameof(UploadTracksRequestHandler))]
    public class UpdateAlbumMetadataRequestHandlerTests
    {
        private readonly UpdateAlbumMetadataRequestHandler _handler;
        private readonly Mock<IDatabase> _db = new Mock<IDatabase>();

        public UpdateAlbumMetadataRequestHandlerTests()
        {
            _handler = new UpdateAlbumMetadataRequestHandler(_db.Object);
        }

        [Fact]
        public async Task Handle_Returns_The_Updated_Metadata()
        {
            var request = new UpdateAlbumMetadataRequest
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
