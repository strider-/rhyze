using Microsoft.AspNetCore.Mvc;
using Moq;
using Rhyze.API.Requests;
using Rhyze.API.Controllers;
using Rhyze.API.Models;
using Rhyze.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Controllers
{
    [Trait(nameof(Controllers), nameof(AlbumsController))]
    public class AlbumsControllerTests : MediatorControllerTestsBase<AlbumsController>
    {
        public AlbumsControllerTests() => UserHasRhyzeId();

        [Fact]
        public async Task IndexAsync_Returns_An_Album_Listing_Page()
        {
            int skip = 10,
                take = 2;
            var query = new GetAlbumsRequest { OwnerId = PrincipalFixture.ExpectedRhyzeId, Skip = skip, Take = take };

            var result = await Controller.IndexAsync(query);

            Mediator.Verify(m => m.Send(It.Is<GetAlbumsRequest>(q =>
                q.OwnerId == PrincipalFixture.ExpectedRhyzeId &&
                q.Skip == skip &&
                q.Take == take
            ), default), Times.Once());
        }

        [Fact]
        public async Task TracksAsync_Returns_Track_View_Models()
        {
            var albumId = new AlbumId("GUMI", "EXIT TUNES PRESENTS Gumissimo from Megpoid");
            var query = new GetAlbumRequest { OwnerId = PrincipalFixture.ExpectedRhyzeId, AlbumId = albumId };
            Mediator.Setup(m => m.Send(It.IsAny<GetAlbumRequest>(), default))
                    .ReturnsAsync(new List<TrackVM> { new TrackVM(), new TrackVM() });

            var result = await Controller.TracksAsync(query);

            Mediator.Verify(m => m.Send(It.Is<GetAlbumRequest>(q =>
                q.OwnerId == PrincipalFixture.ExpectedRhyzeId &&
                q.AlbumId.Name == albumId.Name &&
                q.AlbumId.AlbumArtist == albumId.AlbumArtist
            ), default), Times.Once());
            var r = Assert.IsType<OkObjectResult>(result);
            var body = Assert.IsAssignableFrom<IEnumerable<TrackVM>>(r.Value);
            Assert.Equal(2, body.Count());
        }

        [Fact]
        public async Task TracksAsync_Returns_Not_Found()
        {
            var albumId = new AlbumId("GUMI", "EXIT TUNES PRESENTS Gumissimo from Megpoid");
            var query = new GetAlbumRequest { OwnerId = PrincipalFixture.ExpectedRhyzeId, AlbumId = albumId };

            var result = await Controller.TracksAsync(query);

            Mediator.Verify(m => m.Send(It.Is<GetAlbumRequest>(q =>
                q.OwnerId == PrincipalFixture.ExpectedRhyzeId &&
                q.AlbumId.Name == albumId.Name &&
                q.AlbumId.AlbumArtist == albumId.AlbumArtist
            ), default), Times.Once());
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AlbumMetadataAsync_Returns_The_Album_Metadata()
        {
            var albumId = new AlbumId("YUC'e", "Cinnamon Symphony");
            var query = new GetAlbumMetadataRequest { OwnerId = PrincipalFixture.ExpectedRhyzeId, AlbumId = albumId };
            Mediator.Setup(m => m.Send(It.IsAny<GetAlbumMetadataRequest>(), default))
                    .ReturnsAsync(new AlbumMetadata { });

            var result = await Controller.AlbumMetadataAsync(query);

            Mediator.Verify(m => m.Send(It.Is<GetAlbumMetadataRequest>(q =>
                q.OwnerId == PrincipalFixture.ExpectedRhyzeId &&
                q.AlbumId.Name == albumId.Name &&
                q.AlbumId.AlbumArtist == albumId.AlbumArtist
            ), default), Times.Once());
            var r = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<AlbumMetadata>(r.Value);
        }

        [Fact]
        public async Task AlbumMetadataAsync_Returns_Not_Found()
        {
            var albumId = new AlbumId("YUC'e", "Cinnamon Symphony");
            var query = new GetAlbumMetadataRequest { OwnerId = PrincipalFixture.ExpectedRhyzeId, AlbumId = albumId };
            Mediator.Setup(m => m.Send(It.IsAny<GetAlbumMetadataRequest>(), default))
                    .ReturnsAsync((AlbumMetadata)null);

            var result = await Controller.AlbumMetadataAsync(query);

            Mediator.Verify(m => m.Send(It.Is<GetAlbumMetadataRequest>(q =>
                q.OwnerId == PrincipalFixture.ExpectedRhyzeId &&
                q.AlbumId.Name == albumId.Name &&
                q.AlbumId.AlbumArtist == albumId.AlbumArtist
            ), default), Times.Once());
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_Returns_NoContent()
        {
            var albumId = new AlbumId("REDALiCE", "AKA");
            var query = new DeleteAlbumRequest { OwnerId = PrincipalFixture.ExpectedRhyzeId, Id = albumId };

            var result = await Controller.DeleteAsync(query);

            Assert.IsType<NoContentResult>(result);
            Mediator.Verify(m => m.Send(It.Is<DeleteAlbumRequest>(q =>
                q.OwnerId == PrincipalFixture.ExpectedRhyzeId &&
                q.Id.Name == albumId.Name &&
                q.Id.AlbumArtist == albumId.AlbumArtist
            ), default), Times.Once());
        }

        [Fact]
        public async Task UpdateAlbumMetadataAsync_Returns_The_Updated_Metadata()
        {
            var albumId = new AlbumId("DJ Noriken", "#HYPRFLVX");
            var cmd = new UpdateAlbumMetadataRequest
            {
                OwnerId = PrincipalFixture.ExpectedRhyzeId,
                Id = albumId
            };

            var result = await Controller.UpdateAlbumMetadataAsync(cmd);

            Mediator.Verify(m => m.Send(It.Is<UpdateAlbumMetadataRequest>(q =>
                q.OwnerId == PrincipalFixture.ExpectedRhyzeId &&
                q.Id.Name == albumId.Name &&
                q.Id.AlbumArtist == albumId.AlbumArtist
            ), default), Times.Once());
        }
    }
}