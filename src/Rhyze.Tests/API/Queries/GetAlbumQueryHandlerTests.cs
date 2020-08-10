using Moq;
using Rhyze.API.Commands;
using Rhyze.Core.Models;
using Rhyze.Data;
using Rhyze.Data.Queries;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Queries
{
    [Trait("API.Queries", nameof(GetAlbumQueryHandler))]
    public class GetAlbumQueryHandlerTests
    {
        private readonly Mock<IDatabase> _db = new Mock<IDatabase>();
        private readonly GetAlbumQueryHandler _handler;
        private readonly Guid _ownerId = Guid.NewGuid();

        public GetAlbumQueryHandlerTests()
        {
            _handler = new GetAlbumQueryHandler(_db.Object);
        }

        [Fact]
        public async Task Handle_Executes_The_Expected_Database_Query()
        {
            var albumId = new AlbumId("uma vs. モリモリあつし", "Re：End of a Dream");
            var request = new GetAlbumQuery { AlbumId = albumId, OwnerId = _ownerId };

            await _handler.Handle(request, default);

            _db.Verify(db => db.ExecuteAsync(It.Is<GetAlbumByIdQuery>(q =>
                q.AlbumId.Name == albumId.Name && 
                q.AlbumId.AlbumArtist == albumId.AlbumArtist &&
                q.OwnerId == _ownerId
            )), Times.Once());
        }

        [Fact]
        public async Task Handle_Maps_Track_To_View_Model()
        {
            var request = new GetAlbumQuery { };
            var track = Fixture;
            _db.Setup(db => db.ExecuteAsync(It.IsAny<GetAlbumByIdQuery>()))
               .ReturnsAsync(new[] { track });

            var result = await _handler.Handle(request, default);

            var vm = Assert.Single(result);
            Assert.Equal(track.Id, vm.Id);
            Assert.Equal(track.Album, vm.Album);
            Assert.Equal(track.AlbumArtist, vm.AlbumArtist);
            Assert.Equal(track.Artist, vm.Artist);
            Assert.Equal(track.DiscCount, vm.DiscCount);
            Assert.Equal(track.DiscNo, vm.DiscNo);
            Assert.Equal(track.Duration, vm.Duration);
            Assert.Equal(track.ImageUrl, vm.ImageUrl);
            Assert.Equal(track.PlayCount, vm.PlayCount);
            Assert.Equal(track.Title, vm.Title);
            Assert.Equal(track.TrackCount, vm.TrackCount);
            Assert.Equal(track.TrackNo, vm.TrackNo);
            Assert.Equal(track.Year, vm.Year);
        }

        private Track Fixture => new Track
        {
            Id = Guid.NewGuid(),
            Album = "1",
            AlbumArtist = "2",
            Artist = "3",
            AudioUrl = "4",
            DiscCount = 5,
            DiscNo = 6,
            Duration = 7,
            ImageUrl = "8",
            LastPlayedUtc = DateTime.MinValue,
            OwnerId = Guid.NewGuid(),
            PlayCount = 9,
            Title = "10",
            TrackCount = 11,
            TrackNo = 12,
            UploadedUtc = DateTime.MaxValue,
            Year = 13
        };
    }
}