using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Rhyze.API.Commands;
using Rhyze.API.Controllers;
using Rhyze.API.Models;
using Rhyze.Tests.Fixtures;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Controllers
{
    [Trait(nameof(Controllers), nameof(MediaController))]
    public class MediaControllerTests
    {
        private readonly MediaController _controller;
        private readonly Mock<IMediator> _mediator = new Mock<IMediator>();
        private readonly ClaimsPrincipalFixture _fixture = new ClaimsPrincipalFixture().WithRhyzeId();

        public MediaControllerTests()
        {
            _controller = MediatorControllerGenerator.Create<MediaController>(_fixture, _mediator);
        }

        [Fact]
        public async Task UploadTracksAsync_Returns_Individual_Upload_Results()
        {
            var expectedResult = new List<UploadResult>() { 
                new UploadResult { Filename = "test", Status = UploadStatus.Accepted } 
            };
            var model = new AudioUpload
            {
                Tracks = new FormFileCollection()
            };
            _mediator.Setup(m => m.Send(It.IsAny<UploadTracksCommand>(), default))
                     .ReturnsAsync(expectedResult);

            var result = await _controller.UploadTracksAsync(model);

            Assert.Equal(expectedResult, result);
            _mediator.Verify(m => m.Send(It.Is<UploadTracksCommand>(
                c => c.OwnerId == _fixture.ExpectedRhyzeId && c.Files == model.Tracks), default)
            );
        }
    }
}