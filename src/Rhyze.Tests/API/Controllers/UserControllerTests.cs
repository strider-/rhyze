using Moq;
using Rhyze.API.Controllers;
using Rhyze.API.Models;
using Rhyze.API.Queries;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Controllers
{
    [Trait(nameof(Controllers), nameof(UserController))]
    public class UserControllerTests : MediatorControllerTestsBase<UserController>
    {
        [Fact]
        public async Task MeAsync_Returns_An_Authenticated_User_From_The_Current_Context()
        {
            Mediator.Setup(m => m.Send(It.IsAny<GetMyselfQuery>(), default))
                     .ReturnsAsync(new Me
                     {
                         UserId = PrincipalFixture.ExpectedRhyzeId,
                         Email = PrincipalFixture.ExpectedEmail
                     });

            var result = await Controller.MeAsync();

            Mediator.Verify(m => m.Send(It.Is<GetMyselfQuery>(
                q => q.User == PrincipalFixture.User), default)
            );
            Assert.NotNull(result);
            Assert.Equal(PrincipalFixture.ExpectedEmail, result.Email);
            Assert.Equal(PrincipalFixture.ExpectedRhyzeId, result.UserId);
        }
    }
}
