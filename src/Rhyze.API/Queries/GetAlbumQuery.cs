using MediatR;
using Rhyze.API.Models;
using Rhyze.Core.Models;
using Rhyze.Data;
using Rhyze.Data.Queries;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Commands
{
    public class GetAlbumQuery : RequireAnOwner, IRequest<IEnumerable<Track>>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You need to provide an album name!")]
        public string Name { get; set; }
    }

    public class GetAlbumQueryHandler : IRequestHandler<GetAlbumQuery, IEnumerable<Track>>
    {
        private readonly IDatabase _db;

        public GetAlbumQueryHandler(IDatabase db) => _db = db;

        public async Task<IEnumerable<Track>> Handle(GetAlbumQuery request, CancellationToken cancellationToken)
        {
            return await _db.ExecuteAsync(new GetAlbumByNameQuery(request.OwnerId, request.Name));
        }
    }
}
