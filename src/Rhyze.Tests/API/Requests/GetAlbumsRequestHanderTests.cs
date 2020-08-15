using Moq;
using Rhyze.API.Requests;
using Rhyze.Data;
using Rhyze.Data.Queries;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Requests
{
    [Trait("API.Queries", nameof(GetAlbumsRequest))]
    public class GetAlbumsRequestHanderTests
    {
        private readonly Mock<IDatabase> _db = new Mock<IDatabase>();
        private readonly GetAlbumsRequestHandler _handler;

        public GetAlbumsRequestHanderTests()
        {
            _handler = new GetAlbumsRequestHandler(_db.Object);
        }

        [Fact]
        public async Task Handle_Executes_The_Expected_Database_Query()
        {
            var ownerId = Guid.NewGuid();
            int skip = 5,
                take = 23;
            var request = new GetAlbumsRequest { OwnerId = ownerId, Skip = skip, Take = take };

            await _handler.Handle(request, default);

            _db.Verify(db => db.ExecuteAsync(It.Is<GetAlbumListingQuery>(q =>
                q.OwnerId == ownerId && 
                q.Skip == skip && 
                q.Take == take
            )), Times.Once());
        }
    }
}
