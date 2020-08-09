using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rhyze.API.Models;
using Rhyze.Core.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyze.API.Commands
{
    public class UploadTracksCommand : RequireAnOwner, IRequest<IEnumerable<UploadResult>>
    {
        [FromForm(Name = "tracks")]
        [Required(ErrorMessage = "Please supply one or more audio files in a multipart/form-data request.")]
        public IFormFileCollection Tracks { get; set; }
    }

    public class UploadTracksCommandHandler : IRequestHandler<UploadTracksCommand, IEnumerable<UploadResult>>
    {
        private readonly IUploadService _uploadService;

        public UploadTracksCommandHandler(IUploadService uploadService) => _uploadService = uploadService;

        public async Task<IEnumerable<UploadResult>> Handle(UploadTracksCommand request, CancellationToken cancellationToken)
        {
            var results = new List<UploadResult>();

            foreach (var file in request.Tracks)
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