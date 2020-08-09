using MediatR;
using Rhyze.Core.Interfaces;
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
        private readonly IQueueService _service;

        public DeleteAlbumCommandHandler(IQueueService service) => _service = service;

        public async Task<Unit> Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
        {
            await _service.EnqueueAlbumDeletionAsync(request.Name);

            return Unit.Value;
        }
    }
}