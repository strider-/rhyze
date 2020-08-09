using Rhyze.API.Queries;
using Rhyze.Tests.Fixtures;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Queries
{
    [Trait("API.Queries", nameof(GetMyselfHandler))]
    public class GetMyselfQueryHandlerTests
    {
        private readonly GetMyselfHandler _handler;
        private readonly ClaimsPrincipalFixture _fixture;

        public GetMyselfQueryHandlerTests()
        {
            _fixture = new ClaimsPrincipalFixture().WithRhyzeId();
            _handler = new GetMyselfHandler();
        }

        [Fact]
        public async Task Handle_Returns_The_Authenticated_User()
        {
            var request = new GetMyselfQuery(_fixture.User);

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.Equal(_fixture.ExpectedRhyzeId, result.UserId);
            Assert.Equal(_fixture.ExpectedEmail, result.Email);
        }
    }
}
