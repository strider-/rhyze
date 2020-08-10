using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rhyze.API.Commands;
using Rhyze.API.Models;
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
        public async Task<IEnumerable<Album>> IndexAsync([FromQuery] GetAlbumsQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet("{albumId}")]
        public async Task<IEnumerable<TrackVM>> TracksAsync([FromRoute] GetAlbumQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet("{albumId}/metadata")]
        public async Task<AlbumMetadata> AlbumMetadataAsync([FromRoute] GetAlbumMetadataQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteAlbumCommand cmd)
        {
            await _mediator.Send(cmd);

            return NoContent();
        }
    }
}