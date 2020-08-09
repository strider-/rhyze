using Moq;
using Rhyze.API.Controllers;
using Rhyze.API.Queries;
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
            var query = new GetAlbumsQuery { OwnerId = PrincipalFixture.ExpectedRhyzeId, Skip = skip, Take = take };

            var result = await Controller.IndexAsync(query);

            Mediator.Verify(m => m.Send(It.Is<GetAlbumsQuery>(q =>
                q.OwnerId == PrincipalFixture.ExpectedRhyzeId &&
                q.Skip == skip &&
                q.Take == take
            ), default), Times.Once());
        }
    }
}