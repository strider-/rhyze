using MediatR;
using Rhyze.Core.Interfaces;
using Rhyze.Data;
using Rhyze.Data.Commands;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Commands
{
    public class DeleteAlbumCommand : IRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please specify the album name to delete.")]
        public string Name { get; set; }

        public Guid OwnerId { get; set; }
    }

    public class DeleteAlbumCommandHandler : IRequestHandler<DeleteAlbumCommand, Unit>
    {
        private readonly IQueueService _service;
        private readonly IDatabase _db;

        public DeleteAlbumCommandHandler(IQueueService service, IDatabase db)
        {
            _service = service;
            _db = db;
        }

        public async Task<Unit> Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
        {
            await _db.ExecuteAsync(new SoftDeleteAlbumCommand(request.OwnerId, request.Name));

            await _service.EnqueueAlbumDeletionAsync(request.OwnerId, request.Name);

            return Unit.Value;
        }
    }
}