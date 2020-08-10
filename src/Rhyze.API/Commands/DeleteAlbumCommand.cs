﻿using MediatR;
using Rhyze.API.Models;
using Rhyze.Core.Interfaces;
using Rhyze.Core.Messages;
using Rhyze.Core.Models;
using Rhyze.Data;
using Rhyze.Data.Commands;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Commands
{
    public class DeleteAlbumCommand : RequireAnOwner, IRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please specify the id of the album to delete.")]
        public AlbumId Id { get; set; }
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
            await _db.ExecuteAsync(new SoftDeleteAlbumCommand(request.OwnerId, request.Id));

            await _service.EnqueueAlbumDeletionAsync(new DeleteAlbumMessage
            {
                OwnerId = request.OwnerId,
                AlbumIdValue = request.Id.Value
            });

            return Unit.Value;
        }
    }
}