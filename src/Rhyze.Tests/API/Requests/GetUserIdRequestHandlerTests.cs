using Moq;
using Rhyze.API.Requests;
using Rhyze.Data;
using Rhyze.Data.Queries;
using Rhyze.Tests.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Requests
{
    [Trait("API.Queries", nameof(GetUserIdRequestHandler))]
    public class GetUserIdRequestHandlerTests
    {
        private readonly GetUserIdRequestHandler _handler;
        private readonly Mock<IDatabase> _db = new Mock<IDatabase>();
        private readonly ClaimsPrincipalFixture _fixture;

        public GetUserIdRequestHandlerTests()
        {
            _fixture = new ClaimsPrincipalFixture();

            _handler = new GetUserIdRequestHandler(_db.Object);
        }

        [Fact]
        public async Task Handle_Executes_The_Expected_Database_Query()
        {
            var expectedId = Guid.NewGuid();
            var request = new GetUserIdRequest(_fixture.User);
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