using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rhyze.API.Commands;
using Rhyze.API.Extensions;
using Rhyze.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyze.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MediaController(IMediator mediator) => _mediator = mediator;

        [HttpPost("upload/track")]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
                           MemoryBufferThreshold = int.MaxValue,
                           ValueLengthLimit = int.MaxValue)]
        public async Task<IEnumerable<UploadResult>> UploadTrackAsync([FromForm] AudioUpload model)
        {
            var result = await _mediator.Send(new UploadTracksCommand(
                ownerId: User.UserId(),
                files: model.Tracks
            ));

            return result;
        }
    }
}