using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rhyze.API.Commands;
using Rhyze.API.Queries;
using Rhyze.Core.Models;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> TracksAsync([FromRoute] GetAlbumQuery query)
        {
            var result = await _mediator.Send(query);
            if (result == null || !result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{albumId}/metadata")]

        public async Task<IActionResult> AlbumMetadataAsync([FromRoute] GetAlbumMetadataQuery query)
        {
            var result = await _mediator.Send(query);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost("metadata")]
        public async Task<AlbumMetadata> UpdateAlbumMetadataAsync([FromBody] UpdateAlbumMetadataCommand cmd)
        {
            return await _mediator.Send(cmd);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteAlbumCommand cmd)
        {
            await _mediator.Send(cmd);

            return NoContent();
        }
    }
}