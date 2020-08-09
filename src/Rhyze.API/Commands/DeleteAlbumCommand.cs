using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Commands
{
    public class DeleteAlbumCommand : IRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please specify the album name to delete.")]
        public string Name { get; set; }
    }

    public class DeleteAlbumCommandHandler : IRequestHandler<DeleteAlbumCommand, Unit>
    {
        public Task<Unit> Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }
    }
}
