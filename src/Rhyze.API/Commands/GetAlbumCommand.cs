using MediatR;
using Rhyze.API.Models;
using Rhyze.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Commands
{
    public class GetAlbumCommand : RequireAnOwner, IRequest<IEnumerable<Track>>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You need to provide an album name!")]
        public string Name { get; set; }
    }

    public class GetAlbumCommandHandler : IRequestHandler<GetAlbumCommand, IEnumerable<Track>>
    {
        public Task<IEnumerable<Track>> Handle(GetAlbumCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Enumerable.Empty<Track>());
        }
    }
}
