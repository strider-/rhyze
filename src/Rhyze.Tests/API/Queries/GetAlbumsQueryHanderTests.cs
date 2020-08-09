using Moq;
using Rhyze.API.Queries;
using Rhyze.Data;
using Rhyze.Data.Queries;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Queries
{
    [Trait("API.Queries", nameof(GetAlbumsQuery))]
    public class GetAlbumsQueryHanderTests
    {
        private readonly Mock<IDatabase> _db = new Mock<IDatabase>();
        private readonly GetAlbumsQueryHandler _handler;

        public GetAlbumsQueryHanderTests()
        {
            _handler = new GetAlbumsQueryHandler(_db.Object);
        }

        [Fact]
        public async Task Handle_Executes_The_Expected_Database_Query()
        {
            var ownerId = Guid.NewGuid();
            int skip = 5,
                take = 23;
            var request = new GetAlbumsQuery { OwnerId = ownerId, Skip = skip, Take = take };

            await _handler.Handle(request, default);

            _db.Verify(db => db.ExecuteAsync(It.Is<GetAlbumListingQuery>(q =>
                q.OwnerId == ownerId && 
                q.Skip == skip && 
                q.Take == take
            )), Times.Once());
        }

        [Fact]
        public async Task The_Request_Query_Ensures_Negative_Values_Become_Zero()
        {
            var ownerId = Guid.NewGuid();
            int skip = -15,
                take = -12;
            var request = new GetAlbumsQuery { OwnerId = ownerId, Skip = skip, Take = take };

            await _handler.Handle(request, default);

            _db.Verify(db => db.ExecuteAsync(It.Is<GetAlbumListingQuery>(q =>
                q.OwnerId == ownerId &&
                q.Skip == 0 &&
                q.Take == 0
            )), Times.Once());
        }
    }
}
