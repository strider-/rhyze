using Rhyze.API.Requests;
using Rhyze.Tests.Fixtures;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Requests
{
    [Trait("API.Queries", nameof(GetMyselfRequestHandler))]
    public class GetMyselfRequestHandlerTests
    {
        private readonly GetMyselfRequestHandler _handler;
        private readonly ClaimsPrincipalFixture _fixture;

        public GetMyselfRequestHandlerTests()
        {
            _fixture = new ClaimsPrincipalFixture().WithRhyzeId();
            _handler = new GetMyselfRequestHandler();
        }

        [Fact]
        public async Task Handle_Returns_The_Authenticated_User()
        {
            var request = new GetMyselfRequest(_fixture.User);

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.Equal(_fixture.ExpectedRhyzeId, result.UserId);
            Assert.Equal(_fixture.ExpectedEmail, result.Email);
        }
    }
}
