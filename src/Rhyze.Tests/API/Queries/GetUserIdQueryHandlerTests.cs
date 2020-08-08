using Moq;
using Rhyze.API.Queries;
using Rhyze.Data;
using Rhyze.Data.Queries;
using Rhyze.Tests.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Queries
{
    [Trait("API.Queries", nameof(GetUserIdQueryHandler))]
    public class GetUserIdQueryHandlerTests
    {
        private readonly GetUserIdQueryHandler _handler;
        private readonly Mock<IDatabase> _db = new Mock<IDatabase>();
        private readonly ClaimsPrincipalFixture _fixture;

        public GetUserIdQueryHandlerTests()
        {
            _fixture = new ClaimsPrincipalFixture();

            _handler = new GetUserIdQueryHandler(_db.Object);
        }

        [Fact]
        public async Task Handle_Executes_The_Expected_Database_Query()
        {
            var expectedId = Guid.NewGuid();
            var request = new GetUserIdQuery(_fixture.User);
            _db.Setup(db => db.ExecuteAsync(It.IsAny<GetUserIdFromIdentityQuery>())).ReturnsAsync(expectedId);

            var id = await _handler.Handle(request, default);

            Assert.Equal(expectedId, id);
            _db.Verify(db => db.ExecuteAsync(
                It.Is<GetUserIdFromIdentityQuery>(
                    q => q.Email == _fixture.ExpectedEmail && q.IdentityId == _fixture.ExpectedIdentityId)),
            Times.Once());
        }
    }
}