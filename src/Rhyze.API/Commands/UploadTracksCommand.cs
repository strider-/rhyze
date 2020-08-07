using MediatR;
using Microsoft.AspNetCore.Http;
using Rhyze.API.Models;
using Rhyze.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Commands
{
    public class UploadTracksCommand : IRequest<IEnumerable<UploadResult>>
    {
        public UploadTracksCommand(Guid ownerId, IFormFileCollection files)
        {
            OwnerId = ownerId;
            Files = files;
        }

        public Guid OwnerId { get; }

        public IFormFileCollection Files { get; }
    }

    public class UploadTracksCommandHandler : IRequestHandler<UploadTracksCommand, IEnumerable<UploadResult>>
    {
        private readonly IUploadService _uploadService;

        public UploadTracksCommandHandler(IUploadService uploadService) => _uploadService = uploadService;

        public async Task<IEnumerable<UploadResult>> Handle(UploadTracksCommand request, CancellationToken cancellationToken)
        {
            var results = new List<UploadResult>();

            foreach (var file in request.Files)
            {
                var result = new UploadResult { Filename = file.FileName };

                var error = await _uploadService.UploadTrackAsync(request.OwnerId, file.ContentType, file.OpenReadStream());

                if (error != null)
                {
                    result.Status = UploadStatus.Rejected;
                    result.StatusDetail = error.ToString();
                }
                else
                {
                    result.Status = UploadStatus.Accepted;
                }

                results.Add(result);
            }

            return results.AsEnumerable();
        }
    }
}