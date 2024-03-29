﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rhyze.API.Models;
using Rhyze.API.Requests;
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

        [HttpPost("upload/tracks")]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
                           MemoryBufferThreshold = int.MaxValue,
                           ValueLengthLimit = int.MaxValue)]
        public async Task<IEnumerable<UploadResult>> UploadTracksAsync([FromForm] UploadTracksRequest cmd)
        {
            return await _mediator.Send(cmd);
        }
    }
}