﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rhyze.API.Commands;
using Rhyze.API.Extensions;
using Rhyze.API.Queries;
using Rhyze.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyze.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlbumsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AlbumsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IEnumerable<Album>> IndexAsync([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            return await _mediator.Send(new GetAlbumsQuery(User.UserId(), skip, take));
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteAlbumCommand cmd)
        {
            cmd.OwnerId = User.UserId();

            await _mediator.Send(cmd);

            return NoContent();
        }
    }
}