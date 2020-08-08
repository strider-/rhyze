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
            _controller = MediatorControllerGenerator.Create<UserController>(_fixture, _mediator);
        }

        [Fact]
        public async Task MeAsync_Returns_An_Authenticated_User_From_The_Current_Context()
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetAuthenticatedUserQuery>(), default))
                     .ReturnsAsync(new AuthenticatedUser
                     {
                         UserId = _fixture.ExpectedRhyzeId,
                         Email = _fixture.ExpectedEmail
                     });

            var result = await _controller.MeAsync();

            _mediator.Verify(m => m.Send(It.Is<GetAuthenticatedUserQuery>(
                q => q.User == _fixture.User), default)
            );
            Assert.NotNull(result);
            Assert.Equal(_fixture.ExpectedEmail, result.Email);
            Assert.Equal(_fixture.ExpectedRhyzeId, result.UserId);
        }
    }
}
