using MediatR;
using Moq;
using Rhyze.API.Controllers;
using Rhyze.API.Models;
using Rhyze.API.Queries;
using Rhyze.Tests.Fixtures;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Controllers
{
    [Trait(nameof(Controllers), nameof(UserController))]
    public class UserControllerTests
    {
        private readonly UserController _controller;
        private readonly Mock<IMediator> _mediator = new Mock<IMediator>();
        private readonly ClaimsPrincipalFixture _fixture = new ClaimsPrincipalFixture();

        public UserControllerTests()
        {
            _controller = new UserController(_mediator.Object);
        }

        [Fact]
        public async Task MeAsync_Returns_Authenticated_User()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetAuthenticatedUserQuery>(), default))
                     .ReturnsAsync(new AuthenticatedUser
                     {
                         UserId = _fixture.ExpectedRhyzeId,
                         Email = _fixture.ExpectedEmail
                     });

            var result = await _controller.MeAsync();

            Assert.NotNull(result);
            Assert.Equal(_fixture.ExpectedEmail, result.Email);
            Assert.Equal(_fixture.ExpectedRhyzeId, result.UserId);
        }
    }
}
