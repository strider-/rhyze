﻿using Microsoft.AspNetCore.Http;
using Moq;
using Rhyze.API.Requests;
using Rhyze.API.Controllers;
using Rhyze.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Rhyze.Tests.API.Controllers
{
    [Trait(nameof(Controllers), nameof(MediaController))]
    public class MediaControllerTests : MediatorControllerTestsBase<MediaController>
    {
        public MediaControllerTests() => UserHasRhyzeId();

        [Fact]
        public async Task UploadTracksAsync_Returns_Individual_Upload_Results()
        {
            var expectedResult = new List<UploadResult>() {
                new UploadResult { Filename = "test", Status = UploadStatus.Accepted }
            };
            var model = new UploadTracksRequest
            {
                OwnerId = PrincipalFixture.ExpectedRhyzeId,
                Tracks = new FormFileCollection()
            };
            Mediator.Setup(m => m.Send(It.IsAny<UploadTracksRequest>(), default))
                     .ReturnsAsync(expectedResult);

            var result = await Controller.UploadTracksAsync(model);

            Assert.Equal(expectedResult, result);
            Mediator.Verify(m => m.Send(It.Is<UploadTracksRequest>(
                c => c.OwnerId == PrincipalFixture.ExpectedRhyzeId && c.Tracks == model.Tracks), default)
            );
        }
    }
}